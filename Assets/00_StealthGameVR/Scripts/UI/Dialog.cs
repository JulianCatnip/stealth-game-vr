using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dialog : MonoBehaviour
{
    public List<GameObject> pages;
    private int activePageIndex;

    void Start()
    {
        foreach(GameObject page in this.pages)
        {
            page.SetActive(false);
        }

        this.pages[0].SetActive(true);
        this.activePageIndex = 0;
    }

    public void GoToPage(int pageIndex)
    {
        this.pages[this.activePageIndex].SetActive(false);
        this.activePageIndex = pageIndex;
        this.pages[this.activePageIndex].SetActive(true);
    }
    
    public void GoToNextPage()
    {
        this.pages[this.activePageIndex].SetActive(false);
        this.activePageIndex++;
        this.pages[this.activePageIndex].SetActive(true);
    }

    public void GoToPreviousPage()
    {
        this.pages[this.activePageIndex].SetActive(false);
        this.activePageIndex--;
        this.pages[this.activePageIndex].SetActive(true);
    }

    public void ResetPages()
    {
        foreach(GameObject page in this.pages)
        {
            page.SetActive(false);
        }

        this.pages[0].SetActive(true);
        this.activePageIndex = 0;
    }

    public void PlayIntro()
    {
        SceneManager.LoadScene("Intro");
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
