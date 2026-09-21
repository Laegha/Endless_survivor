using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _masterVolumeText;
    [SerializeField] TextMeshProUGUI _sfxVolumeText;
    [SerializeField] TextMeshProUGUI _musicVolumeText;
    [SerializeField] GameObject _controlsObj;
    [SerializeField] GameObject _crtObj;
    public void SetValues()
    {
        _masterVolumeText.text = Application.persistentDataPath;    
    }
    private void Update()
    {

    }
}