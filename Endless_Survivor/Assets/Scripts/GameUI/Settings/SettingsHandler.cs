using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class SettingsHandler
{
    static string _settingsPath = Path.Combine(Application.streamingAssetsPath, "player_prefs.json");

    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] FullScreenPassRendererFeature _crtRenderFeature;
    bool _usingCustomControls = false;
    bool _usingCrt;
    public const string masterVolumeGroup = "Master";
    public const string sfxVolumeGroup = "SFX";
    public const string musicVolumeGroup = "Music";

    SettingsJsonInfo _jsonInfo
    {
        get
        {
            SettingsJsonInfo result = new();
            result.master = GetVolume01(masterVolumeGroup);
            result.sfx = GetVolume01(sfxVolumeGroup);
            result.music = GetVolume01(musicVolumeGroup);
            result.customControls = _usingCustomControls;
            result.crt = _usingCrt;
            return result;
        }
    }

    public bool UsingCustomControls { get { return _usingCustomControls; } }
    public bool UsingCrt { get { return _usingCrt; } }
    public FullScreenPassRendererFeature CrtRenderFeature { get { return _crtRenderFeature; } }

    public async void GetJsonValues()
    {
        string playerPrefs = await Utility.ReadJson(_settingsPath);
        var settingsInfo = JsonConvert.DeserializeObject<SettingsJsonInfo>(playerPrefs);
        SetVolume(masterVolumeGroup, settingsInfo.master);
        SetVolume(sfxVolumeGroup, settingsInfo.sfx);
        SetVolume(musicVolumeGroup, settingsInfo.music);
        _usingCustomControls = settingsInfo.customControls;
        _usingCrt = settingsInfo.crt;
        _crtRenderFeature.SetActive(_usingCrt);
    }

    public void SetVolume(string volumeGroup, float volume)
    {
        _audioMixer.SetFloat(volumeGroup, Mathf.Log10(volume) * 20);
        UpdateJson();
        //_audioMixer.SetFloat(volumeGroup, Mathf.Log10(20) * volume);
    }
    public float GetVolume01(string volumeGroup)
    {
        float volume;
        _audioMixer.GetFloat(volumeGroup, out volume);
        volume = Mathf.Pow(10, (volume / 20));
        return volume;
    }
    public void ToggleControls()
    {
        _usingCustomControls = !_usingCustomControls;
        UpdateJson();
    }
    public void ToggleCrt()
    {
        _usingCrt = !_usingCrt;
        _crtRenderFeature.SetActive(_usingCrt);
        UpdateJson();
    }
    void UpdateJson()
    {
        SettingsJsonInfo jsonInfo = _jsonInfo;
        string newJson = JsonConvert.SerializeObject(jsonInfo, Formatting.Indented);
        File.WriteAllText(_settingsPath, newJson);
    }
}
