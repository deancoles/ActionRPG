using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

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
    public GameObject winOptionsHolder;
    public GameObject inGameEnemySelectPanel;


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
        playerMovement.rb.velocity = Vector2.zero;                      // Stop player movement physics
        playerMovement.anim.SetFloat("horizontal", 0f);                 // Force horizontal to 0
        playerMovement.anim.SetFloat("vertical", 0f);                   // Force vertical to 0
        playerMovement.enabled = false;                                 // Freeze player controls
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
        winOptionsHolder.SetActive(true);                               // Activate the buttons after fade-in
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");                             // Returns to Main Menu level
    }

    public void RestartLevel()
    {
        winOptionsHolder.SetActive(false);                              // Hide the win menu
        inGameEnemySelectPanel.SetActive(true);                         // Show enemy choice
    }

    public void ChooseEnemiesDuringGame(int amount)
    {
        enemySpawns = amount;
        ClearExistingEnemies();
        SpawnEnemies();
        UpdateEnemiesText();
        winUI.SetActive(true);                                          // Re-enable WinUI
        winCanvasGroup.alpha = 0f;                                      // Hide the Win Message text again                                      
        inGameEnemySelectPanel.SetActive(false);                        // Hide the panel again
        playerMovement.ResetToStart();                                  // Move player back to start
        StartCoroutine(EnablePlayerMovementDelayed());                  // Wait before allowing movement
    }

    public void QuitGame()
    {
        Application.Quit();                                             // Quit Games
        Debug.Log("Quit Game");                                         // Used for testing purposes in Unity editor
    }

    void ClearExistingEnemies()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }
    }

    private IEnumerator EnablePlayerMovementDelayed()
    {
        yield return new WaitForSeconds(0.5f);                          // Wait 0.5 seconds
        playerMovement.enabled = true;                                  // Then allow player to move
    }
}
