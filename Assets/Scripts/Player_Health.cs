using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Controls the player's health and updates the on-screen display.
public class Player_Health : MonoBehaviour
{
    public int currentHealth;           // Player's current health value.
    public int maxHealth;               // Player's maximum health value.
    public TMP_Text healthText;         // UI element used to display health.
    public Animator healthTextAnim;     // Animator for visual health feedback.


    // Initialises the health display when the game starts.
    private void Start()
    {
        healthText.text = "HP: " + currentHealth + " / " + maxHealth;   
    }

    // Adjusts the player's health by a given amount.
    public void ChangeHealth(int amount)
    {
        currentHealth += amount;                                        // Apply the health change (positive to heal, negative to damage).
        healthTextAnim.Play("TextUpdate");                              // Play feedback animation to indicate health has changed.
        healthText.text = "HP: " + currentHealth + " / " + maxHealth;   // Update the health text to reflect the new health.

        // If health reaches zero or less, disable the player GameObject.
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    // Resets player health upon restarting a level.
    public void ResetHealth()
    {
        currentHealth = maxHealth;                                      // Restore health back to maximum.
        healthText.text = "HP: " + currentHealth + " / " + maxHealth;   // Update the health UI to full health.
    }
}
