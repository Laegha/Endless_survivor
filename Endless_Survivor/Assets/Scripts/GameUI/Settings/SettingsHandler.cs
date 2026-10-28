using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using ControlsType = SettingsJsonInfo.ControlsType;

[Serializable]
public class SettingsHandler
{
    static string _settingsPath = "player_prefs.json";

    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] FullScreenPassRendererFeature _crtRenderFeature;

    ControlsType _controlsType = ControlsType.StaticJoystic;
    bool _usingCrt;
    string _selectedChar;
    public const string masterVolumeGroup = "Master";
    public const string sfxVolumeGroup = "SFX";
    public const string musicVolumeGroup = "Music";
    const string defaultCharFileName = "TheBoogeyman";
    SettingsJsonInfo _jsonInfo
    {
        get
        {
            SettingsJsonInfo result = new();
            result.master = GetVolume01(masterVolumeGroup);
            result.sfx = GetVolume01(sfxVolumeGroup);
            result.music = GetVolume01(musicVolumeGroup);
            result.controlsType = _controlsType;
            result.crt = _usingCrt;
            result.selectedCharFileName = _selectedChar;
            return result;
        }
    }
    ControlsType _controlsTypeMaxValue => Enum.GetValues(typeof(ControlsType)).Cast<ControlsType>().Last();
    public ControlsType ControlsType { get { return _controlsType; } }
    public bool UsingCrt { get { return _usingCrt; } }
    public FullScreenPassRendererFeature CrtRenderFeature { get { return _crtRenderFeature; } }

    public async void GetJsonValues()
    {
        string settingsData = await Utility.ReadJson(_settingsPath);
        var settingsInfo = JsonConvert.DeserializeObject<SettingsJsonInfo>(settingsData);
        SetVolume(masterVolumeGroup, settingsInfo.master, false);
        SetVolume(sfxVolumeGroup, settingsInfo.sfx, false);
        SetVolume(musicVolumeGroup, settingsInfo.music, false);
        _controlsType = settingsInfo.controlsType;
        _usingCrt = settingsInfo.crt;
        _crtRenderFeature.SetActive(_usingCrt);

        LoadStartingChar(settingsInfo);
        
        
        //UpdateJson();
    }
    async void LoadStartingChar(SettingsJsonInfo settingsInfo)
    {
        while (GameManager.gm.UnlockedElementHelper.UnlockedCharacters == null || GameManager.gm.UnlockedElementHelper.UnlockedCharacters.Count == 0)
            await Task.Yield();

        CharacterData selectedChar = GameManager.gm.UnlockedElementHelper.UnlockedCharacters.Find(c => c.element.name == settingsInfo.selectedCharFileName)?.element;
        
        if(selectedChar == null)
            selectedChar = GameManager.gm.UnlockedElementHelper.UnlockedCharacters.Find(c => c.element.name == defaultCharFileName)?.element;

        _selectedChar = selectedChar.name;

        GameManager.gm.SelectedCharacter = selectedChar;
    }

    public void SetVolume(string volumeGroup, float volume, bool updateJson = true)
    {
        _audioMixer.SetFloat(volumeGroup, Mathf.Log10(volume) * 20);
        if(updateJson)
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
    public void ControlsTypeUp()
    {
        _controlsType++;
        if (_controlsType > _controlsTypeMaxValue)
            _controlsType = 0;
        UpdateJson();
    }
    public void ControlsTypeDown()
    {
        _controlsType--;
        if (_controlsType < 0)
            _controlsType = _controlsTypeMaxValue;
        UpdateJson();
    }
    public void ToggleCrt()
    {
        _usingCrt = !_usingCrt;
        _crtRenderFeature.SetActive(_usingCrt);
        UpdateJson();
    }
    public void ChangeSelectedCharFile(CharacterData character)
    {
        _selectedChar = character.name;
        UpdateJson();
    }
    void UpdateJson()
    {
        SettingsJsonInfo jsonInfo = _jsonInfo;
        string newJson = JsonConvert.SerializeObject(jsonInfo, Formatting.Indented);
        Utility.WriteJson(_settingsPath, newJson);
    }
}
