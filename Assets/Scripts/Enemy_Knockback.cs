using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls enemy knockback when hit by the player.
public class Enemy_Knockback : MonoBehaviour
{
    private Rigidbody2D rb;                                                             // Reference to the enemy's Rigidbody2D.
    private Enemy_Movement enemy_Movement;                                              // Reference to movement script to change states.


    private void Start()
    {   
        rb = GetComponent<Rigidbody2D>();                                               // Get Rigidbody component.
        enemy_Movement = GetComponent<Enemy_Movement>();                                // Get Enemy_Movement script.
    }

    public void Knockback(Transform playerTransform, float KnockbackForce, float knockbackTime, float stunTime)
    {
        enemy_Movement.ChangeState(EnemyState.Knockback);                               // Switch enemy to knockback state.
        StartCoroutine(StunTimer(knockbackTime, stunTime));                             // Begin the knockback and stun sequence.
        Vector2 direction = (transform.position - playerTransform.position).normalized; // Calculate direction away from player.
        rb.velocity = direction * KnockbackForce;                                       // Apply force to enemy.
    }

    IEnumerator StunTimer(float knockbackTime,float stunTime)
    {
        yield return new WaitForSeconds(knockbackTime);                                 // Wait while knockback happens.
        rb.velocity = Vector2.zero;                                                     // Stop enemy movement.
        yield return new WaitForSeconds(stunTime);                                      // Wait during stuntime.
        enemy_Movement.ChangeState(EnemyState.Idle);                                    // Enemy can move again.
    }
}
