using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.XR;

// Controls enemy movement: chases the player while in range, returns to start when not chasing.
public class Enemy_Movement : MonoBehaviour
{
    public float speed;                 // How fast the enemy moves.
    private int facingDirection = -1;   // The direction the enemy is facing, -1 is left, 1 is right.
    private EnemyState enemyState;      // Tracks current state of enemy for animations.
    private Rigidbody2D rb;             // Enemy's Rigidbody for movement.    
    private Transform player;           // Reference to the player’s position.
    private Animator anim;              // Reference to the Animator component.


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
    }

    // Handles chasing the player or returning to start.
    void Update()
    {
        if (enemyState == EnemyState.Chasing)
        {
            // if player is on the right but enemy is facing left or player is on the left but enemy is facing right
            if (player.position.x > transform.position.x && facingDirection == -1 || player.position.x < transform.position.x && facingDirection == 1)
            { 
                    Flip();
            }
            Vector2 direction = (player.position - transform.position).normalized;          // Move towards the player's position.
            rb.velocity = direction * speed;
        } 
    }

    // Flip the enemy sprite
    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }


    // Triggered when the player enters the enemy's detection range.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (player == null)
            {
                player = collision.transform;
            }
            ChangeState(EnemyState.Chasing);        // Start chasing the player.
        }
    }

    // Triggered when the player leaves the enemy's detection range.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rb.velocity = Vector2.zero;             // Stop movement immediately.
            ChangeState(EnemyState.Idle);           // Return to idle state.
        }
    }

    void ChangeState(EnemyState newstate)
    {
        // Exit the current animation
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", false);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", false);

        enemyState = newstate;      // Update our current state

        // Update the new animation
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", true);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", true);
    }
}

// List of states the enemy can be in.
public enum EnemyState
{
    Idle,
    Chasing,
}
