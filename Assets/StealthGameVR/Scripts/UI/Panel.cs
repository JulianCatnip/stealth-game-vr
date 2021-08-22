using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{   
    // Canvas reference
    private Canvas canvas = null;
    // Reference to MenuManager
    private MenuManager menuManager = null;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    /**
     * Set ups each panel and hides its components.
     * */
    public void Setup(MenuManager _menuManager)
    {
        this.menuManager = _menuManager;
        HideCanvas();
    }

    /**
     * Enables the canvas and its components..
     */
    public void ShowCanvas()
    {
        canvas.enabled = true;
    }

    /**
     * Disables the canvas and its components.
     */
    public void HideCanvas()
    {
        canvas.enabled = false;
    }
}
