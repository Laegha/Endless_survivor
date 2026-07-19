using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomControlsToggle : MonoBehaviour
{
    [SerializeField] GameObject _toggleFill;
    private void Start()
    {
        _toggleFill.SetActive(GameManager.gm.UsingCustomControls);
        
    }
    public void ToggleControls()
    {
        GameManager.gm.UsingCustomControls = !GameManager.gm.UsingCustomControls;
        _toggleFill.SetActive(GameManager.gm.UsingCustomControls);
    }
}
