using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script controls player movement, sprite flipping, and animation.
public class Player_Movement : MonoBehaviour
{
    private Vector2 startingPosition;           // Player starting posiiton on the map
    public float speed = 5;                     // Speed at which the player moves (public makes this editable in the Inspector)
    public int facingDirection = 1;             // 1 for right, -1 for left
    public Rigidbody2D rb;                      // Reference to the player's Rigidbody2D component
    public Animator anim;                       // Reference to the Animator for controlling animations
    private bool isKnockedBack;                 // Tracks if the player is currently being knocked back.
    public Player_Combat player_Combat;         // Reference to the Player_Combat script controlling attacks.


    // Saves the starting position of the player at the start of the scene.
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();       // Get the Rigidbody2D component.
        anim = GetComponent<Animator>();        // Get the Animator component.
        startingPosition = transform.position;  // Save the player's starting position.                             
    }

    // Resets the player position back to the starting point.
    public void ResetToStart()
    {
        transform.position = startingPosition; 
    }

    // Handles player input for attacking.
    private void Update()
    {
        // If the slash button is pressed, attempt an attack.
        if (Input.GetButtonDown("Slash"))
        {
            player_Combat.Attack();             // Call the attack function from Player_Combat script.
        }
    }

    // Handles player movement and flipping based on input.
    void FixedUpdate()
    {
        // Always get horizontal and vertical input.
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Always update animation parameters for movement.
        anim.SetFloat("horizontal", Mathf.Abs(horizontal));
        anim.SetFloat("vertical", Mathf.Abs(vertical));

        // Only allow movement if the player is not currently knocked back.
        if (isKnockedBack == false)
        {
            rb.velocity = new Vector2(horizontal, vertical) * speed;    // Set movement velocity.

            // Flip the sprite if changing movement direction.
            if (horizontal > 0 && transform.localScale.x < 0 ||
                horizontal < 0 && transform.localScale.x > 0)
            {
                Flip();
            }
        }
    }

    // Flips the player's sprite horizontally.
    void Flip()
    {
        facingDirection *= -1; 
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    // Handles knockback when the player is hit by an enemy.
    public void Knockback(Transform enemy, float force, float stunTime)
    {
        isKnockedBack = true;                                                   // Mark player as knocked back.
        Vector2 direction = (transform.position - enemy.position).normalized;   // Calculate knockback direction.
        rb.velocity = direction * force;                                        // Apply knockback force.
        StartCoroutine(KnockbackCounter(stunTime));                             // Start a stun coroutine.
    }

    // Coroutine to handle the stun duration and regain player control.
    IEnumerator KnockbackCounter(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);  // Wait for the stun time to pass.
        rb.velocity = Vector2.zero;                 // Stop any movement.
        isKnockedBack = false;                      // Allow the player to move again.
    }
}
