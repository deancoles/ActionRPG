using UnityEngine;
using UnityEngine.SceneManagement;

// Manages main menu interactions: starting game, choosing enemies, exiting game.
public class Main_Menu_Manager : MonoBehaviour
{
    public GameObject mainMenuPanel;        // Reference to the main menu UI panel.
    public GameObject enemySelectPanel;     // Reference to the enemy selection UI panel.
    public GameObject exitConfirmPanel;     // Reference to the exit confirmation UI panel.
    public int enemiesToSpawn;              // Stores the number of enemies the player has selected to spawn.

    // Starts the game by hiding main menu and showing enemy selection panel.
    public void StartGame()
    {
        mainMenuPanel.SetActive(false);     // Hide the main menu panel.
        enemySelectPanel.SetActive(true);   // Show the enemy selection panel.
    }

    // Stores selected enemy count and loads the RPG scene.
    public void ChooseEnemies(int amount)
    {
        enemiesToSpawn = amount;                                // Store the selected number of enemies.
        PlayerPrefs.SetInt("EnemiesToSpawn", enemiesToSpawn);   // Save the selected number of enemies using PlayerPrefs.
        SceneManager.LoadScene("RPG");                          // Load the game level.
    }

    // Brings up the exit confirmation menu
    public void ExitGame()
    {
        mainMenuPanel.SetActive(false);     // Hide the main menu panel.
        exitConfirmPanel.SetActive(true);   // Show the exit confirmation panel.
    }

    // Confirms and exits the application.
    public void ConfirmExitYes()
    {
        Application.Quit();         // Quit the application.
        Debug.Log("Quit Game");     // Log a message to the console confirming the game quit.
    }

    // Cancels exiting and returns to main menu.
    public void ConfirmExitNo()
    {
        exitConfirmPanel.SetActive(false);  // Hide the exit confirmation panel.
        mainMenuPanel.SetActive(true);      // Show the main menu panel again.
    }

    // Returns from enemy selection screen to main menu.
    public void ReturnToMainMenu()
    {
        enemySelectPanel.SetActive(false);  // Hide the enemy selection panel.
        mainMenuPanel.SetActive(true);      // Show the main menu panel again.
    }
}
