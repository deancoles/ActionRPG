using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Manages overall game state: spawning enemies, win conditions, UI control.
public class Game_Manager : MonoBehaviour
{
    public int enemySpawns = 1;                 // Total number of enemies to spawn.
    private int enemiesDefeated = 0;            // How many enemies have been defeated so far.
    public GameObject enemyPrefab;              // Reference to the enemy prefab.
    public Transform[] spawnPoints;             // Array of spawn locations.
    public TMP_Text enemiesText;                // UI text showing enemies defeated.
    public GameObject winUI;                    // UI object for the win message.
    private CanvasGroup winCanvasGroup;         // CanvasGroup to control fade.
    public Player_Health playerHealth;          // Reference to Player Health script.
    private Player_Movement playerMovement;     // Reference to Player Movement script.
    public GameObject winOptionsHolder;         // Holder for win screen buttons.
    public GameObject inGameEnemySelectPanel;   // Panel for choosing enemy count after winning.


    void Start()
    {
        enemySpawns = PlayerPrefs.GetInt("EnemiesToSpawn", enemySpawns);    // Load from PlayerPrefs if exists.
        SpawnEnemies();                                                     // Spawn enemies at start.
        UpdateEnemiesText();                                                // Set up initial UI text.
        winCanvasGroup = winUI.GetComponent<CanvasGroup>();                 // Get CanvasGroup for win message.
        winCanvasGroup.alpha = 0;                                           // Make win UI invisible at start.
        playerMovement = FindObjectOfType<Player_Movement>();               // Find the player movement script.
    }

    // Spawns enemies at random spawn points.
    void SpawnEnemies()
    {
        List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);    // Make a copy of all spawn points.

        //  Spawn one enemy at SpawnPoint1
        if (availableSpawnPoints.Count > 0)
        {
            Instantiate(enemyPrefab, availableSpawnPoints[0].position, Quaternion.identity);    // Force spawn at first point.
            enemiesDefeated = 0;                                                                // Reset defeated count.
            enemiesText.text = "Enemies\n" + enemiesDefeated + " / " + enemySpawns;             // Update enemy text.

            availableSpawnPoints.RemoveAt(0);                                                   // Remove first spawn point from list so it's not picked again.
        }

        int enemiesLeftToSpawn = enemySpawns - 1;   // Randomly spawn the rest

        // Loop through the number of enemies that still need to be spawned
        for (int i = 0; i < enemiesLeftToSpawn; i++)
        {
            // If there are no available spawn points left.
            if (availableSpawnPoints.Count == 0)
            {
                Debug.LogWarning("Not enough spawn points available!");
                break;  // Exit the loop because we cannot spawn any more enemies.
            }
            
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);                              // Pick random spawn point.
            Instantiate(enemyPrefab, availableSpawnPoints[randomIndex].position, Quaternion.identity);  // Spawn enemy.
            availableSpawnPoints.RemoveAt(randomIndex);                                                 // Prevent duplicate spawn points.
        }
    }

    // Called when an enemy is defeated.
    public void EnemyDefeated()
    {
        enemiesDefeated++;      // Increase defeated counter.
        UpdateEnemiesText();    // Update UI text.

        // If all enemies defeated.
        if (enemiesDefeated >= enemySpawns)
        {
            WinGame();          // Trigger win sequence.
        }
    }

    // Updates the Enemies UI text.
    void UpdateEnemiesText()
    {
        enemiesText.text = "Defeated\n" + enemiesDefeated + " / " + enemySpawns;    // Format defeated enemies text
    }

    // Handles player winning the game.
    void WinGame()
    {
        playerMovement.rb.velocity = Vector2.zero;          // Stop player movement immediately.
        playerMovement.anim.SetFloat("horizontal", 0f);     // Force horizontal to 0.
        playerMovement.anim.SetFloat("vertical", 0f);       // Force vertical to 0.
        playerMovement.enabled = false;                     // Disable movement control.
        StartCoroutine(WinMessageFadeIn());                 // Start fading in the win message.
    }

    // Fades in the Win UI message smoothly.
    IEnumerator WinMessageFadeIn()
    {
        yield return new WaitForSeconds(1f);                            // Wait time after final enemy
        float duration = 1f;                                            // Fade in time
        float elapsed = 0f;                                             // Tracks how much time has passed since starting the fade.

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            winCanvasGroup.alpha = Mathf.Clamp01(elapsed / duration);   // Smooth fade
            yield return null;
        }

        winCanvasGroup.alpha = 1;                                       // Ensure fully visible at end.
        winOptionsHolder.SetActive(true);                               // Show win menu options.
    }
    
    // Returns player to main menu scene.
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");     // Load the Main Menu scene.
    }

    // Restarts the level (shows enemy choice).
    public void RestartLevel()
    {
        winOptionsHolder.SetActive(false);          // Hide the win menu.
        inGameEnemySelectPanel.SetActive(true);     // Show panel to choose new enemy.
    }

    // Handles player selecting a number of enemies after restarting.
    public void ChooseEnemiesDuringGame(int amount)
    {
        enemySpawns = amount;                               // Set new enemy spawn number.
        ClearExistingEnemies();                             // Remove old enemies.
        SpawnEnemies();                                     // Spawn new enemies.
        UpdateEnemiesText();                                // Refresh enemy text.
        winUI.SetActive(true);                              // Re-enable WinUI.
        winCanvasGroup.alpha = 0f;                          // Reset Win Message visibility.                               
        inGameEnemySelectPanel.SetActive(false);            // Hide enemy choice panel.
        playerMovement.ResetToStart();                      // Reset player position.
        playerHealth.ResetHealth();                         // Restore player health.
        StartCoroutine(EnablePlayerMovementDelayed());      // Wait before allowing player movement.
    }

    // Quits the application.
    public void QuitGame()
    {
        Application.Quit();             // Quit Game.
        Debug.Log("Quit Game");         // Used for testing inside Unity editor.
    }

    // Removes all enemies from the scene.
    void ClearExistingEnemies()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy); // Destroy each enemy object
        }
    }

    // Waits before re-enabling player control
    private IEnumerator EnablePlayerMovementDelayed()
    {
        yield return new WaitForSeconds(0.5f);      // Wait half a second.
        playerMovement.enabled = true;              // Re-enable player controls.
    }
}