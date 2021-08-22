using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{   
    
    /**
     * If the play button is pressed, it opens the game.
     * */
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /**
     * Lets the player exit the game
     * */
    public void ExitGame()
    {
        Application.Quit();
    }
}
