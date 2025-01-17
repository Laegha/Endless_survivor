using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionMenu : MonoBehaviour
{
    [SerializeField] GameObject _menu;

    public TMP_Dropdown resolutionDropdown; // Dropdown para seleccionar resoluciones

    private Resolution[] resolutions;

    void Start()
    {
        // Obtener las resoluciones disponibles
        resolutions = Screen.resolutions;

        // Poblar el dropdown con las resoluciones
        resolutionDropdown.ClearOptions();
        foreach (Resolution res in resolutions)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(res.width + "x" + res.height));
        }

        // Asignar evento al dropdown y al toggle
        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
    }

    public void ChangeResolution(int index)
    {
        Resolution res = resolutions[index];
        RefreshRate refreshRate = new RefreshRate();
        Screen.SetResolution(res.width, res.height, FullScreenMode.Windowed, refreshRate);
    }

    public void ReturnToMenu()
    {
        _menu.SetActive(true);
        gameObject.SetActive(false);
    }
}
