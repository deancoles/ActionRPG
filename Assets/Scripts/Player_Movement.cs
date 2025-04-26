using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script controls player movement, sprite flipping, and animation
public class Player_Movement : MonoBehaviour
{
    private Vector2 startingPosition;                               // Player starting posiiton on the map
    public float speed = 5;                                         // Speed at which the player moves (public makes this editable in the Inspector)
    public int facingDirection = 1;                                 // 1 for right, -1 for left

    public Rigidbody2D rb;                                          // Reference to the player's Rigidbody2D component
    public Animator anim;                                           // Reference to the Animator for controlling animations

    private bool isKnockedBack;                                     // Tracks if the player is currently being knocked back.
    public Player_Combat player_Combat;                             // Reference to the Player_Combat script controlling attacks.

    
    // Save where player starts
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();                           // Rigidbody grab
        anim = GetComponent<Animator>();                            // Animator grab
        startingPosition = transform.position;                                         
    }

    // Reset player posiiton back to start
    public void ResetToStart()
    {
        transform.position = startingPosition;
    }

    // Handles player input for attacks.
    private void Update()
    {
        // If the slash button is pressed, attempt an attack.
        if (Input.GetButtonDown("Slash"))
        {
            player_Combat.Attack();
        }

    }

    void FixedUpdate()
    {
        // Handle normal movement if player is not stunned.
        if (isKnockedBack == false)
        {
            // Get player input on the horizontal (A/D or Left/Right) and vertical (W/S or Up/Down) axes
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Check if the player has changed horizontal direction, and flip the sprite if needed
            if (horizontal > 0 && transform.localScale.x < 0 ||
                horizontal < 0 && transform.localScale.x > 0)
            {
                Flip();
            }

            // Update animator parameters to control Idle and Walking animations
            anim.SetFloat("horizontal", Mathf.Abs(horizontal));
            anim.SetFloat("vertical", Mathf.Abs(vertical));

            rb.velocity = new Vector2(horizontal, vertical) * speed;    // Set the Rigidbody2D's velocity based on input and speed
        }
    }

    // Flips the player's sprite by scaling it negatively on the X-axis
    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    // Handles knockback when the player is hit by an enemy.
    public void Knockback(Transform enemy, float force, float stunTime)
    {
        isKnockedBack = true;
        Vector2 direction = (transform.position - enemy.position).normalized;   // Calculate knockback direction.
        rb.velocity = direction * force;                                        // Apply knockback force.
        StartCoroutine(KnockbackCounter(stunTime));                             // Begin a short stun period where the player can't move.
    }

    // Waits for stun time to expire before regaining control.
    IEnumerator KnockbackCounter(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        rb.velocity = Vector2.zero;
        isKnockedBack = false;
    }
}
