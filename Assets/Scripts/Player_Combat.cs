using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the player's attack mechanics.
public class Player_Combat : MonoBehaviour
{
    public Transform attackPoint;       // The point where the player's attack originates.
    public float weaponRange = 1;       // Range of weapon for attacking.
    public float knockbackForce = 50;   // How far enemy knockback will send them on screen.
    public float knockbackTime = .15f;  // Duration of knockback.
    public float stunTime = .3f;        // How long enemy is stunned for after knockback.
    public int damage = 1;              // Damage dealt per hit.
    public Animator anim;               // Animator to control the attack animations.
    public float cooldown = 2;          // Time between allowed attacks.
    private float timer;                // Tracks cooldown timer.
    public LayerMask enemyLayer;        // Layer to detect enemies.


    // Called once per frame to update the attack cooldown timer.
    private void Update()
    {
        // Reduce attack cooldown over time.
        if (timer > 0)
        {
            timer -= Time.deltaTime;    // Decrease the cooldown timer.
        }
    }

    // Triggers an attack if cooldown has finished.
    public void Attack()
    {
        // If the player can attack.
        if (timer <= 0)
        {
            anim.SetBool("isAttacking", true);  // Play the attacking animation.
            timer = cooldown;                   // Reset the attack cooldown timer.
        }
    }

    // Deals damage to any enemies in attack range.
    public void DealDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, enemyLayer);                   // Detect enemies within attack radius.

        // Loop through every enemy found inside the attack radius.
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.isTrigger) continue;                                                                                  // Ignore trigger-only colliders.

            // If at least one enemy was detected.
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
        anim.SetBool("isAttacking", false); // Return player to idle or movement animation.
    }

    // Draws the attack range in Scene view when selected.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;                                  // Set gizmo colour to blue.
        Gizmos.DrawWireSphere(attackPoint.position, weaponRange);   // Draw the attack range circle.
    }
}
