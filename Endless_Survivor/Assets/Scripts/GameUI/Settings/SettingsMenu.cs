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

        SetSlider(_masterSlider, SettingsHandler.masterVolumeGroup);
        SetSlider(_sfxSlider, SettingsHandler.sfxVolumeGroup);
        SetSlider(_musicSlider, SettingsHandler.musicVolumeGroup);

        _customControlsToggle.SetActive(GameManager.gm.SettingsHandler.UsingCustomControls);
        _crtToggle.SetActive(GameManager.gm.SettingsHandler.UsingCrt);
    }
    public void HideMenu()
    {
        _menuObj.SetActive(false);
    }
    void SetSlider(Slider slider, string valueName)
    {
        float volume = GameManager.gm.SettingsHandler.GetVolume01(valueName);
        slider.value = volume;
    }
    public void SetMasterVolume()
    {
        GameManager.gm.SettingsHandler.SetVolume(SettingsHandler.masterVolumeGroup, _masterSlider.value);
    }
    public void SetSFXVolume()
    {
        GameManager.gm.SettingsHandler.SetVolume(SettingsHandler.sfxVolumeGroup, _sfxSlider.value);
    }
    public void SetMusicVolume()
    {
        GameManager.gm.SettingsHandler.SetVolume(SettingsHandler.musicVolumeGroup, _musicSlider.value);
    }
    public void ToggleCustomControls()
    {
        GameManager.gm.SettingsHandler.ToggleControls();
        _customControlsToggle.SetActive(GameManager.gm.SettingsHandler.UsingCustomControls);
        if (PlayerControl.pc == null)
            return;
        PlayerControl.pc.InputReader.UpdateMobileControls();
    }
    public void ToggleCRTFilter()
    {
        GameManager.gm.SettingsHandler.ToggleCrt();
        _crtToggle.SetActive(GameManager.gm.SettingsHandler.UsingCrt);
    }
}
