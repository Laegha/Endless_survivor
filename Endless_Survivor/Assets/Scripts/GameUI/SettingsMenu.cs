using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public void SetMasterVolume(float volume)
    {
        GameManager.gm.SetVolume("Master", volume);
    }
    public void SetSFXVolume(float volume)
    {
        GameManager.gm.SetVolume("SFX", volume);
    }
    public void SetMusicVolume(float volume)
    {
        GameManager.gm.SetVolume("Music", volume);
    }
}
