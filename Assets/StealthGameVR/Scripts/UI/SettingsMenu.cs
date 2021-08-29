using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{

    // reference to master audio mixer
    public AudioMixer audioMixer;

    /**
     * Sets the volume of the game with the slider.
     */
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
}
