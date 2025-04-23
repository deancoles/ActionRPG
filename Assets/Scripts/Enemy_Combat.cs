using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles enemy's ability to damage the player when colliding.
public class Enemy_Combat : MonoBehaviour
{
    public int damage = 1;  // How much damage this enemy does.

    // Runs when the enemy collides with player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If object collided with is the player
        if (collision.gameObject.tag == "Player")
        { 
            collision.gameObject.GetComponent<Player_Health>().ChangeHealth(-damage);    // Damage the player.
        }
    }
}
