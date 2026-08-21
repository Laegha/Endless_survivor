using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] GameObject _menuObj;
    [SerializeField] Slider _masterSlider;
    [SerializeField] Slider _sfxSlider;
    [SerializeField] Slider _musicSlider;
    [SerializeField] GameObject _customControlsToggle;
    [SerializeField] GameObject _crtToggle;
    public void DisplayMenu()
    {
        _menuObj.SetActive(true);

        //Set timescale to 0 with the manager
        SetSlider(_masterSlider, "Master");
        SetSlider(_sfxSlider, "SFX");
        SetSlider(_musicSlider, "Music");

        _crtToggle.SetActive(GameManager.gm.CrtRenderFeature.isActive);
        _customControlsToggle.SetActive(GameManager.gm.UsingCustomControls);
    }
    public void HideMenu()
    {
        _menuObj.SetActive(false);
    }
    void SetSlider(Slider slider, string valueName)
    {
        float volume = GameManager.gm.GetVolume01(valueName);
        slider.value = volume;
    }
    public void SetMasterVolume()
    {
        GameManager.gm.SetVolume("Master", _masterSlider.value);
    }
    public void SetSFXVolume()
    {
        GameManager.gm.SetVolume("SFX", _sfxSlider.value);
    }
    public void SetMusicVolume()
    {
        GameManager.gm.SetVolume("Music", _musicSlider.value);
    }
    public void ToggleCustomControls()
    {
        GameManager.gm.UsingCustomControls = !GameManager.gm.UsingCustomControls;
        _customControlsToggle.SetActive(GameManager.gm.UsingCustomControls);
        if (PlayerControl.pc == null)
            return;
        PlayerControl.pc.InputReader.UpdateMobileControls();
    }
    public void ToggleCRTFilter()
    {
        GameManager.gm.CrtRenderFeature.SetActive(!GameManager.gm.CrtRenderFeature.isActive);
        _crtToggle.SetActive(GameManager.gm.CrtRenderFeature.isActive);
    }
}
