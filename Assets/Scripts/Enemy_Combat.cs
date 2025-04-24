using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles enemy's ability to damage the player when colliding.
public class Enemy_Combat : MonoBehaviour
{
    public int damage = 1;  // How much damage this enemy does.
    public Transform attackPoint;
    public float weaponRange;
    public LayerMask playerLayer;

    // Runs when the enemy collides with player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If object collided with is the player
        if (collision.gameObject.tag == "Player")
        { 
            collision.gameObject.GetComponent<Player_Health>().ChangeHealth(-damage);                   // Damage the player.
        }
    }

    public void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer); // Check for any player colliders within the attack radius.

        if (hits.Length > 0)
        {
            hits[0].GetComponent<Player_Health>().ChangeHealth(-damage);
        }
    }
}
