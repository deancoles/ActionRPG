using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu_Manager : MonoBehaviour
{

    public GameObject mainMenuPanel;
    public GameObject enemySelectPanel;
    public GameObject exitConfirmPanel;
    public int enemiesToSpawn;                                  // Number of enemies chosen

    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        enemySelectPanel.SetActive(true);
    }

    public void ChooseEnemies(int amount)
    {
        enemiesToSpawn = amount;                                // Choose numbe rof enemies to spawn
        PlayerPrefs.SetInt("EnemiesToSpawn", enemiesToSpawn);   // Save choice
        SceneManager.LoadScene("RPG");                          // Load level
    }

    public void ExitGame()
    {
        mainMenuPanel.SetActive(false);
        exitConfirmPanel.SetActive(true);
    }

    public void ConfirmExitYes()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void ConfirmExitNo()
    {
        exitConfirmPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        enemySelectPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
