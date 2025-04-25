using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    [Header("Enemy Tracking")]
    public int enemySpawns;                 // Total number of enemies to spawn
    private int enemiesDefeated = 0;        // How many enemies have been defeated so far
    public GameObject enemyPrefab;          // Reference to the enemy prefab
    public Transform[] spawnPoints;         // Array of spawn locations

    [Header("UI References")]
    public TMP_Text enemiesText;            // UI text showing enemies defeated
    public GameObject winUI;                // UI object for the win message


    void Start()
    {
        SpawnEnemies();                     // Spawn enemies at start
        UpdateEnemiesText();                // Set up initial UI text
        winUI.SetActive(false);             // Hide win message at start
    }

    // Spawns enemies at random spawn points
    void SpawnEnemies()
    {
        for (int i = 0; i < enemySpawns; i++)
        {
            // Pick a random spawn point
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    // Call this when an enemy dies
    public void EnemyDefeated()
    {
        enemiesDefeated++;                  // Increase defeated counter
        UpdateEnemiesText();                // Update UI text

        if (enemiesDefeated >= enemySpawns)
        {
            WinGame();                      // If all enemies are defeated, win the game
        }
    }

    // Updates the Enemies UI text
    void UpdateEnemiesText()
    {
        enemiesText.text = "Defeated\n" + enemiesDefeated + " / " + enemySpawns;
    }

    // Handles winning the game
    void WinGame()
    {
        winUI.SetActive(true);             // Show the win message
    }
}
