using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    // Checks if the game is currently paused or not
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape)) {

            if (GameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }

    }
    /**
     * Resumes the game.
     * */
    public void Resume()
    {
        // Disables the pause menu
        pauseMenuUI.SetActive(false);
        // Sets time back to normal
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    
    /**
     * Pauses the game.
     * */ 
    public void Pause()
    {
        // Enables the pause menu
        pauseMenuUI.SetActive(true);
        // Freezes the Game
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
   
    
    public void LoadMenu()
    {   
        // Adjusts Time back to normal
        Time.timeScale = 1f;
        // Loads menu
        SceneManager.LoadScene("MainMenu");
    }

    /**
     * Quits the game.
     */ 
    public void QuitGame()
    {
        Application.Quit();
    }
}
