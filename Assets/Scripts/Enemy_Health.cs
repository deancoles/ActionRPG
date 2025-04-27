using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles enemy health and destruction when defeated.
public class Enemy_Health : MonoBehaviour
{
    public int currentHealth;               // The current health value of the enemy.
    public int maxHealth;                   // The maximum health the enemy can have.
    private Game_Manager gameManager;       // Reference to the Game_Manager script.


    private void Start()
    {
        currentHealth = maxHealth;                          // Set current health to maximum at the start.
        gameManager = FindObjectOfType<Game_Manager>();     // Find the GameManager in the scene.
    }

    // Changes health by a given amount.
    public void ChangeHealth(int amount)
    {
        currentHealth += amount;            // Adjust health by amount.

        // Ensures current health cannot exceed maximum
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // If health reaches zero or less, destroy the GameObject.
        else if (currentHealth <= 0)
        {
            gameManager.EnemyDefeated();    // Tell GameManager we defeated an enemy.
            Destroy(gameObject);            // Destroy the enemy object.
        }
    }
}
