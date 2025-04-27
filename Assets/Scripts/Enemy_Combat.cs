using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles enemy's ability to damage the player when colliding.
public class Enemy_Combat : MonoBehaviour
{
    public int damage = 1;          // How much damage this enemy does to the player.
    public Transform attackPoint;   // The position where the enemy's attack is centered.
    public float weaponRange;       // How far the attack can reach.
    public float knockbackForce;    // Force applied to the player when hit.
    public float stunTime;          // How long the player is stunned after knockback.
    public LayerMask playerLayer;   // Layer that defines what is considered a player.


    // Deals damage and knockback to the player if they are within attack range.
    public void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer); // Check for any player colliders within the attack radius.

        // If at least one player is hit
        if (hits.Length > 0)
        {
            hits[0].GetComponent<Player_Health>().ChangeHealth(-damage);                                // Damage the player.
            hits[0].GetComponent<Player_Movement>().Knockback(transform, knockbackForce, stunTime);     // Apply knockback to player.
        }
    }
}
