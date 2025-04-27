using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls enemy knockback and stun after being hit.
public class Enemy_Knockback : MonoBehaviour
{
    private Rigidbody2D rb;                     // Reference to the enemy's Rigidbody2D.
    private Enemy_Movement enemy_Movement;      // Reference to Enemy Movement script.


    private void Start()
    {   
        rb = GetComponent<Rigidbody2D>();                   // Get Rigidbody component.
        enemy_Movement = GetComponent<Enemy_Movement>();    // Get Enemy_Movement script.
    }

    // Applies knockback to enemy.
    public void Knockback(Transform playerTransform, float KnockbackForce, float knockbackTime, float stunTime)
    {
        enemy_Movement.ChangeState(EnemyState.Knockback);                               // Switch enemy to knockback state.
        StartCoroutine(StunTimer(knockbackTime, stunTime));                             // Begin the knockback and stun sequence.
        Vector2 direction = (transform.position - playerTransform.position).normalized; // Get knockback direction.
        rb.velocity = direction * KnockbackForce;                                       // Apply knockback force.
    }

    // Handles stun timer after knockback.
    IEnumerator StunTimer(float knockbackTime,float stunTime)
    {
        yield return new WaitForSeconds(knockbackTime);     // Wait for knockback time.
        rb.velocity = Vector2.zero;                         // Stop enemy movement.
        yield return new WaitForSeconds(stunTime);          // Wait for stun time.
        enemy_Movement.ChangeState(EnemyState.Idle);        // Switch back to idle.
    }
}
