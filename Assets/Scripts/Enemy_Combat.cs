using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles enemy's ability to damage the player when colliding.
public class Enemy_Combat : MonoBehaviour
{
    public int damage = 1;  // How much damage this enemy does.
    public Transform attackPoint;
    public float weaponRange;
    public float knockbackForce;
    public float stunTime;
    public LayerMask playerLayer;


    public void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer); // Check for any player colliders within the attack radius.

        if (hits.Length > 0)
        {
            hits[0].GetComponent<Player_Health>().ChangeHealth(-damage);
            hits[0].GetComponent<Player_Movement>().Knockback(transform, knockbackForce, stunTime);
        }
    }
}
