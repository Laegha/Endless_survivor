using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ModDownloadMenu : MonoBehaviour
{
    [SerializeField] GameObject _menuObj;
    [SerializeField] GameObject _workshopObj;
    [SerializeField] GameObject _downloadedObj;
    [SerializeField] ModDisplay _modDisplay;
    [SerializeField] ModDisplay _moreInfoModDisplay;
    [SerializeField] GameObject _moreInfoBgObj;
    [SerializeField] Transform _modDisplayContainer;
    [SerializeField] TextMeshProUGUI _currPageDisplay;
    int _displayingPages => (int)Mathf.Ceil(_displayingMods.Count / _modsPerPage);
    List<ModInfo> _displayingMods = new List<ModInfo>();
    List<GameObject> _activeModDisplays = new List<GameObject>();

    int _currentPage;
    const int _modsPerPage = 4;
    public void DisplayMenu()
    {
        _menuObj.SetActive(true);
        _currentPage = 0;
        RefreshModList();
    }
    public void RefreshModList()
    {
        foreach (var modDisplay in _activeModDisplays)
        {
            Destroy(modDisplay);
        }

        for (int i = 0; i < _modsPerPage; i++)
        {
            int modIndex = _currentPage * _modsPerPage + i;
            if (_displayingMods.Count <= modIndex)
                break;
            ModInfo createdDisplayInfo = _displayingMods[i];
            ModDisplay modDisplay = GameObject.Instantiate(_modDisplay);
            modDisplay.transform.SetParent(_modDisplayContainer);
            modDisplay.DisplayMod(createdDisplayInfo, this);
            _activeModDisplays.Add(modDisplay.gameObject);
        }
    }
    public void DisplayModWorkshop()
    {
        _workshopObj.SetActive(true);
        _downloadedObj.SetActive(false);
    }
    public void DisplayDownloadedMods()
    {
        _downloadedObj.SetActive(true);
        _workshopObj.SetActive(false);

    }
    void GoToNextPage()
    {
        _currentPage++;
        if(_currentPage >= _displayingPages)
            _currentPage = 0;
        RefreshModList();
    }
    void GoToPreviousPage()
    {
        _currentPage--;
        if (_currentPage < 0)
            _currentPage = _displayingPages -1;
        RefreshModList();
    }
    public void DisplayMoreModInfo(ModInfo modInfo)
    {
        _moreInfoModDisplay.gameObject.SetActive(true);
        _moreInfoBgObj.SetActive(true);
        _moreInfoModDisplay.DisplayMod(modInfo, this);
    }
    public void UnDisplayMoreModInfo()
    {
        _moreInfoModDisplay.gameObject.SetActive(false);
        _moreInfoBgObj.SetActive(false);
    }
    void SetPageDisplay()
    {
        _currPageDisplay.text = $"Page {_currentPage + 1} / {_displayingPages}";
    }
}
