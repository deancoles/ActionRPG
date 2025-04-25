using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Combat : MonoBehaviour
{
    public Transform attackPoint;       // The point where the player's attack originates.
    public float weaponRange = 1;       // Range of weapon for attacking
    public float knockbackForce = 50;   // How far enemy knockback will send thme on screen
    public float knockbackTime = .15f;  // Duration of knockback
    public float stunTime = .3f;        // How long enemy is stunned for after knockback
    public int damage = 1;              // Damage dealt per hit.
    public Animator anim;               // Animator to control the attack animations.
    public float cooldown = 2;          // Time between allowed attacks.
    private float timer;                // Tracks cooldown timer.
    public LayerMask enemyLayer;        // Layer to detect enemies.


    private void Update()
    {
        // Reduce attack cooldown over time.
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    // Attack if cooldown is ready.
    public void Attack()
    {
        if (timer <= 0)
        {
            anim.SetBool("isAttacking", true);
            timer = cooldown;
        }
    }


    public void DealDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, enemyLayer);                   // Detect enemies in range.

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.isTrigger) continue;                                                                                  // Ignore trigger-only colliders.

            if (enemies.Length > 0)
            {
                enemies[0].GetComponent<Enemy_Health>().ChangeHealth(-damage);                                              // Damage the enemy.
                enemies[0].GetComponent<Enemy_Knockback>().Knockback(transform, knockbackForce, knockbackTime, stunTime);   // Apply knockback to enemy.
            }
        }
    }

    // Called at the end of the attack animation to reset the attack state.
    public void FinishAttacking()
    {
        anim.SetBool("isAttacking", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, weaponRange);
    }

}
