using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Controls the player's health and updates the on-screen display.
public class Player_Health : MonoBehaviour
{

    public int currentHealth;                                           // Player's current health value
    public int maxHealth;                                               // Player's maximum health value

    public TMP_Text healthText;                                         // The on-screen health display text
    public Animator healthTextAnim;                                     // Animator used to play a feedback animation when health text updates

    
    // Sets the initial health text display upon beginning
    private void Start()
    {
        healthText.text = "HP: " + currentHealth + " / " + maxHealth;   // Refresh the on-screen health text to reflect the new health value.
    }

    
    // Adjusts the player's health by a given amount.
    public void ChangeHealth(int amount)
    {
        currentHealth += amount;                                        // Apply the health change (positive to heal, negative to damage).
        healthTextAnim.Play("TextUpdate");                              // Play feedback animation to indicate health has changed.

        healthText.text = "HP: " + currentHealth + " / " + maxHealth; 

        // If health reaches zero or less, disable the player GameObject.
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
