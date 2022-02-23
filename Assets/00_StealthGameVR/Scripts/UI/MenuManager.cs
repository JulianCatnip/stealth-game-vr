using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{   
    private GameObject startPanel;
    private GameObject settingsPanel;

    void Start()
    {
        this.startPanel = this.transform.Find("StartPanel").gameObject;
        this.settingsPanel = this.transform.Find("SettingsPanel").gameObject;
        this.startPanel.SetActive(true);
        this.settingsPanel.SetActive(false);
    }
    
    /**
     * If the "start game" button is pressed, load the game scene.
     * */
    public void EnterGame()
    {
        SceneManager.LoadScene("Intro");
    }

    /**
     * If the "settings" button is pressed, enable the settings;
     * */
    public void EnterSettings()
    {
        this.startPanel.SetActive(false);
        this.settingsPanel.SetActive(true);
    }

    /**
     * If the "back" button is pressed, return to the start panel;
     * */
    public void ExitSettings()
    {
        this.startPanel.SetActive(true);
        this.settingsPanel.SetActive(false);
    }

    /**
     * If the "exit" button is pressed, exit game;
     * */
    public void ExitGame()
    {
        Application.Quit();
    }
}
