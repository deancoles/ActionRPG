using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

// Controls enemy movement: chases the player while in range, returns to start when not chasing.
public class Enemy_Movement : MonoBehaviour
{
    public float speed;                 // Movement speed of the enemy.
    public float attackRange = 2;       // How close the player needs to be for an attack to trigger.
    public float attackCooldown = 2;    // Time delay between consecutive enemy attacks.
    private float attackCooldownTimer;  // Tracks how much time remains until next attack.

    public float playerDetectRange = 5; // Radius used to detect the player's presence.
    public Transform detectionPoint;    // Origin point of detection circle.
    public LayerMask playerLayer;       // Only detects objects on the player layer.
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

    // Detect player and handle enemy behaviour based on current state.
    void Update()
    {
        // Only allow normal behaviour if not knocked back.
        if ((enemyState != EnemyState.Knockback))
        {
            CheckForPlayer();

            // Reduce cooldown timer
            if (attackCooldownTimer > 0)
            {
                attackCooldownTimer -= Time.deltaTime;      // Countdown attack cooldown.
            }

            if (enemyState == EnemyState.Chasing)
            {
                Chase();                                    // Move towards player.
            }
            // Stay still while attacking
            else if (enemyState == EnemyState.Attacking)
            {
                rb.velocity = Vector2.zero;                 // Stop moving while attacking.
            }
        }
    }

    // Chase the player
    void Chase()
    {
        // if player is on the right but enemy is facing left or player is on the left but enemy is facing right
        if (player.position.x > transform.position.x && facingDirection == -1 || player.position.x < transform.position.x && facingDirection == 1)
        {
            Flip();
        }

        Vector2 direction = (player.position - transform.position).normalized;          // Move towards the player's position.
        rb.velocity = direction * speed;
    }

    // Flip the enemy sprite
    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }


    // Detects player within sight range and switches between chase or attack.
    private void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectRange, playerLayer);

        if (hits.Length > 0)
        {
            player = hits[0].transform;

            // Attack if within range and not on cooldown
            if (Vector2.Distance(transform.position, player.position) <= attackRange && attackCooldownTimer <= 0)
            {
                attackCooldownTimer = attackCooldown;       // Reset the cooldown
                ChangeState(EnemyState.Attacking);
            }
            // Chase the player
            else if (Vector2.Distance(transform.position, player.position) > attackRange && enemyState != EnemyState.Attacking)
            {
                ChangeState(EnemyState.Chasing);
            }
        }
        else
        {
            rb.velocity = Vector2.zero;                    // Stop movement immediately.
            ChangeState(EnemyState.Idle);
        }
    }

    // Handles transitions between Idle, Chasing, and Attacking animations.
    public void ChangeState(EnemyState newstate)
    {
        // Exit the current animation
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", false);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", false);
        else if (enemyState == EnemyState.Attacking)
            anim.SetBool("isAttacking", false);

        enemyState = newstate;                            // Update our current state

        // Update the new animation
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", true);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", true);
        else if (enemyState == EnemyState.Attacking)
            anim.SetBool("isAttacking", true);
    }

    // Draw a red circle indicating the enemy sight in scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionPoint.position, playerDetectRange);
    }
}

// List of states the enemy can be in.
public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
    Knockback,
}
