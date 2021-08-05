using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour
{
    // Reference to text on canvas
    public Text Textfield;

    /**
     * Changes the text displayed on the canvas to the text input
     */
    public void SetText(string text)
    {
        Textfield.text = text;
    }
}
