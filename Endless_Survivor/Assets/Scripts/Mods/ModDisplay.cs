using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModDisplay : MonoBehaviour
{
    [SerializeField] ScalableToTargetImage _modIconImage;
    [SerializeField] TextMeshProUGUI _modTitleText;
    [SerializeField] TextMeshProUGUI _modCreatorText;
    [SerializeField] TextMeshProUGUI _modDescriptionText;
    [SerializeField] GameObject _downloadButton;
    [SerializeField] GameObject _downloadingButtonReplace;
    [SerializeField] GameObject _deleteButton;
    ModInfo _modInfo;
    ModDownloadMenu _menu;

    public void DisplayMod(ModInfo modInfo, ModDownloadMenu menu)
    {
        _menu = menu;
        _modInfo = modInfo;
        if(_modTitleText != null)
            _modTitleText.text = modInfo.modTitle;
        if(_modIconImage != null)
            _modIconImage.ChangeImageSprite(modInfo.modIcon);
        if(_modCreatorText != null)
            _modCreatorText.text = modInfo.modCreatorName;
        if(_modDescriptionText != null)
            _modDescriptionText.text = modInfo.modDescription;
        //if mod is donwloaded, change the image of the downlaod btn or smth
        bool isDownloaded = ModManager.mm.IsModDownloaded(modInfo);
        _downloadButton.SetActive(!isDownloaded);
        _deleteButton.SetActive(isDownloaded);
        Debug.Log("Mod " + _modInfo.modTitle + " is downloaded: " + isDownloaded);
    }
    public async void DownloadMod()
    {
        _downloadButton.SetActive(false);
        _downloadingButtonReplace.SetActive(true);
        await ModManager.mm.DownloadMod(_modInfo);
        _menu.RefreshModList();
        _downloadingButtonReplace.SetActive(false);
        _deleteButton.SetActive(true);
    }
    public void DeleteMod()
    {
        _deleteButton.SetActive(false);
        _downloadButton.SetActive(true);
        ModManager.mm.DeleteMod(_modInfo);
        _menu.RefreshModList();
    }
    public void DisplayMoreInfo()
    {
        _menu.DisplayMoreModInfo(_modInfo);
    }
}
