using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

// This script controls player movement, sprite flipping, and animation
public class Player_Movement : MonoBehaviour
{
    public float speed = 5;                                         // Speed at which the player moves (public makes this editable in the Inspector)
    public int facingDirection = 1;                                 // 1 for right, -1 for left

    public Rigidbody2D rb;                                          // Reference to the player's Rigidbody2D component
    public Animator anim;                                           // Reference to the Animator for controlling animations



    // Update is called 50 times per second
    void FixedUpdate()
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

    // Flips the player's sprite by scaling it negatively on the X-axis
    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}
