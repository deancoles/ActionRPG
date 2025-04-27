using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls enemy movement: chases the player while in range, returns to start when not chasing.
public class Enemy_Movement : MonoBehaviour
{
    public float speed;                     // Movement speed of the enemy.
    public float attackRange = 2;           // How close the player needs to be for an attack to trigger.
    public float attackCooldown = 2;        // Time delay between consecutive enemy attacks.
    private float attackCooldownTimer;      // Tracks how much time remains until next attack.
    public float playerDetectRange = 5;     // Radius used to detect the player's presence.
    public Transform detectionPoint;        // Origin point of detection circle.
    public LayerMask playerLayer;           // Only detects objects on the player layer.
    private int facingDirection = -1;       // The direction the enemy is facing, -1 is left, 1 is right. 
    private EnemyState enemyState;          // Tracks current state of enemy for animations.
    private Rigidbody2D rb;                 // Enemy's Rigidbody for movement.    
    private Transform player;               // Reference to the player’s position.
    private Animator anim;                  // Reference to the Animator component.


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   // Get reference to Rigidbody2D.
        anim = GetComponent<Animator>();    // Get reference to Animator.
        ChangeState(EnemyState.Idle);       // Start enemy in idle state.
    }


    // Detect player and handle enemy behaviour based on current state.
    void Update()
    {
        // Only act if not being knocked back.
        if ((enemyState != EnemyState.Knockback))
        {
            CheckForPlayer();                               // Look for player nearby.                              

            // If attack cooldown is active
            if (attackCooldownTimer > 0)
            {
                attackCooldownTimer -= Time.deltaTime;      // Countdown timer.
            }

            // If chasing, move towards player.
            if (enemyState == EnemyState.Chasing)
            {
                Chase();                                    
            }

            // If attacking, stop moving.
            else if (enemyState == EnemyState.Attacking)
            {
                rb.velocity = Vector2.zero;                 // Freeze movement while attacking.
            }
        }
    }


    // Move enemy towards the player.
    void Chase()
    {
        // Check if enemy needs to flip based on player's position.
        if (player.position.x > transform.position.x && facingDirection == -1 || player.position.x < transform.position.x && facingDirection == 1)
        {
            Flip();
        }

        Vector2 direction = (player.position - transform.position).normalized;      // Move towards the player's position.
        rb.velocity = direction * speed;                                            // Set velocity to move towards player.
    }


    // Flip the enemy sprite.
    void Flip()
    {
        facingDirection *= -1;  
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);    // Mirror the sprite.
    }


    // Checks for nearby player and updates enemy state.
    private void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectRange, playerLayer);            // Detect all players in range.

        // If a player was detected
        if (hits.Length > 0)
        {
            player = hits[0].transform;                 // Target the first player detected.

            
            if (Vector2.Distance(transform.position, player.position) <= attackRange && attackCooldownTimer <= 0)
            {
                attackCooldownTimer = attackCooldown;   // Reset attack cooldown.
                ChangeState(EnemyState.Attacking);      // Switch to attacking state.
            }
            
            else if (Vector2.Distance(transform.position, player.position) > attackRange && enemyState != EnemyState.Attacking)
            {
                ChangeState(EnemyState.Chasing);        // Continue chasing if out of attack range.
            }
        }

        else
        {
            rb.velocity = Vector2.zero;         // Stop moving.
            ChangeState(EnemyState.Idle);       // Switch to idle state.
        }
    }


    // Changes the enemy's current state and updates animations.
    public void ChangeState(EnemyState newstate)
    {
        // Turn off the current state's animation
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", false);              // Exit idle animation.
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", false);           // Exit chasing animation.
        else if (enemyState == EnemyState.Attacking)
            anim.SetBool("isAttacking", false);         // Exit attacking animation.

        enemyState = newstate;                          // Update our current state

        // Enable the new state's animation
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", true);               // Enter idle animation.
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", true);            // Enter chasing animation.
        else if (enemyState == EnemyState.Attacking)
            anim.SetBool("isAttacking", true);          // Enter attacking animation.
    }


    // Draws a red circle in the editor to show detection range.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;                                           // Set gizmo colour to red.
        Gizmos.DrawWireSphere(detectionPoint.position, playerDetectRange);  // Draw detection circle.
    }
}

// List of possible enemy states.
public enum EnemyState
{
    Idle,           // Standing still.
    Chasing,        // Moving towards player.
    Attacking,      // Currently attacking player.
    Knockback,      // Being knocked backwards by player attack.
}
