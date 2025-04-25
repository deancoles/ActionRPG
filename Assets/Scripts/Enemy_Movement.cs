using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.XR;

// Controls enemy movement: chases the player while in range, returns to start when not chasing.
public class Enemy_Movement : MonoBehaviour
{
    public float speed;                 // How fast the enemy moves.
    public float attackRange = 2;       // How close player has to get before enemy attacks
    public float attackCooldown = 2;    // The time after an attack before a new one can take place
    public float playerDetectRange = 5; // The distance within which enemy will see player
    public Transform detectionPoint;    // Centre point of enemy circle of sight
    public LayerMask playerLayer;       // Only detects players

    private float attackCooldownTimer;  // Tracks how much cooldown remains before next attack.
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

    
    void Update()
    {
        CheckForPlayer();

        // Reduce cooldown timer
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        if (enemyState == EnemyState.Chasing)
        {
            Chase();
        }
        // Stay still while attacking
        else if (enemyState == EnemyState.Attacking)
        {
            rb.velocity = Vector2.zero;
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


    // Checks for player in range and decides whether to chase or attack.
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
    void ChangeState(EnemyState newstate)
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
}
