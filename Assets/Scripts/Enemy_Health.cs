using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles enemy health
public class Enemy_Health : MonoBehaviour
{
    public int currentHealth;                                           // The current health value of the enemy.
    public int maxHealth;                                               // The maximum health the enemy can have.


    private void Start()
    {
        currentHealth = maxHealth;                                      // Set current health to maximum at the start.
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;                                        // Adjust health by amount.

        // Ensures current health cannot exceed maximum
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // If health reaches zero or less, destroy the GameObject.
        else if (currentHealth <= 0)
        {
            Destroy(gameObject);        
        }
    }
}
