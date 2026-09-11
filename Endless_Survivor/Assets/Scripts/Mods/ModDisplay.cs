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
    [SerializeField] GameObject _deleteButton;
    ModInfo _modInfo;
    ModDownloadMenu _menu;

    public void DisplayMod(ModInfo modInfo, ModDownloadMenu menu)
    {
        _menu = menu;
        _modInfo = modInfo;
        _modTitleText.text = modInfo.ModTitle;
        _modIconImage.ChangeImageSprite(modInfo.ModIcon);
        _modCreatorText.text = modInfo.ModCreatorName;
        _modDescriptionText.text = modInfo.ModDescription;
        //if mod is donwloaded, change the image of the downlaod btn or smth
        bool isDownloaded = ModManager.mm.IsModDownloaded(modInfo);
        _downloadButton.SetActive(!isDownloaded);
        _deleteButton.SetActive(isDownloaded);
        DownloadMod();
    }
    public void DownloadMod()
    {
        ModManager.mm.DownloadMod(_modInfo);
        _menu.RefreshModList();
    }
    public void DeleteMod()
    {
        ModManager.mm.DeleteMod(_modInfo);
        _menu.RefreshModList();
    }
    public void DisplayMoreInfo()
    {
        _menu.DisplayMoreModInfo(_modInfo);
    }
}
