using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour
{
    public int enemySpawns = 1;                                             // Total number of enemies to spawn
    private int enemiesDefeated = 0;                                        // How many enemies have been defeated so far
    public GameObject enemyPrefab;                                          // Reference to the enemy prefab
    public Transform[] spawnPoints;                                         // Array of spawn locations
    public TMP_Text enemiesText;                                            // UI text showing enemies defeated
    public GameObject winUI;                                                // UI object for the win message
    private CanvasGroup winCanvasGroup;                                     // CanvasGroup to control fade
    private Player_Movement playerMovement;                                 // Reference to the player's movement script


    void Start()
    {
        enemySpawns = PlayerPrefs.GetInt("EnemiesToSpawn", enemySpawns);    // Load from PlayerPrefs if exists
        SpawnEnemies();                                                     // Spawn enemies at start
        UpdateEnemiesText();                                                // Set up initial UI text
        winCanvasGroup = winUI.GetComponent<CanvasGroup>();                 // Grab CanvasGroup
        winCanvasGroup.alpha = 0;                                           // Ensure invisible at start
        playerMovement = FindObjectOfType<Player_Movement>();               // Find the player movement script
    }

    // Spawns enemies at random spawn points
    void SpawnEnemies()
    {
        List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);

        // 1. GUARANTEE: Spawn one goblin at SpawnPoint1
        if (availableSpawnPoints.Count > 0)
        {
            Instantiate(enemyPrefab, availableSpawnPoints[0].position, Quaternion.identity);
            enemiesDefeated = 0; // Reset defeated count
            enemiesText.text = "Enemies\n" + enemiesDefeated + " / " + enemySpawns;

            availableSpawnPoints.RemoveAt(0); // Remove SpawnPoint1 from list so it's not picked again
        }

        // 2. Randomly spawn the rest
        int enemiesLeftToSpawn = enemySpawns - 1;

        for (int i = 0; i < enemiesLeftToSpawn; i++)
        {
            if (availableSpawnPoints.Count == 0)
            {
                Debug.LogWarning("Not enough spawn points available!");
                break;
            }

            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            Instantiate(enemyPrefab, availableSpawnPoints[randomIndex].position, Quaternion.identity);
            availableSpawnPoints.RemoveAt(randomIndex); // Prevent duplicate spawn points
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
        StartCoroutine(WinMessageFadeIn());
    }

    IEnumerator WinMessageFadeIn()
    {
        yield return new WaitForSeconds(1f);                            // Wait time after final enemy
        float duration = 1f;                                            // Fade in time
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            winCanvasGroup.alpha = Mathf.Clamp01(elapsed / duration);   // Smooth fade
            yield return null;
        }

        winCanvasGroup.alpha = 1;                                       // Ensure fully visible at end
        playerMovement.enabled = false;                                 // Disable player control after winning
    }
}
