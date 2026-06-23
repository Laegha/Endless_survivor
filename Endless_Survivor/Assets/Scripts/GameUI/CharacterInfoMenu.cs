using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _charName;
    [SerializeField] ScalableToTargetImage _charDisplayImage;
    
    [InspectorLabel("Character stats display")]
    [SerializeField] TextMeshProUGUI _maxHPDisplay;
    [SerializeField] TextMeshProUGUI _regenerationDisplay;
    [SerializeField] TextMeshProUGUI _speedDisplay;
    [SerializeField] TextMeshProUGUI _defenseDisplay;

    [InspectorLabel("Starting weapons and items display")]
    [SerializeField] RectTransform _weaponDisplayRow;
    [SerializeField] RectTransform _itemDisplayRow;
    [SerializeField] GameObject _weaponDisplayPrefab;
    [SerializeField] GameObject _itemDisplayPrefab;

    List<GameObject> _displayingWeapons = new();
    List<GameObject> _displayingItems = new();


    public void UpdateInfo(CharacterData newShowingChar)
    {
        _charName.text = newShowingChar.name;
        _charDisplayImage.ChangeImageSprite(newShowingChar.MenuImage);

        _maxHPDisplay.text = newShowingChar.PlayerStats.InitialHP + "";
        _regenerationDisplay.text = newShowingChar.PlayerStats.HPRegeneration + "";
        _defenseDisplay.text = newShowingChar.PlayerStats.Defense + "";
        _speedDisplay.text = newShowingChar.PlayerStats.MaxSpeed + "";

        var displayingWeaponsCopy = new List<GameObject>(_displayingWeapons);
        foreach (var existingWeapon in displayingWeaponsCopy)
        {
            _displayingWeapons.Remove(existingWeapon);
            Destroy(existingWeapon);
        }
        
        foreach(var newCharStartingWeapon in newShowingChar.InitialWeapons)
        {
            if(newCharStartingWeapon.WeaponDisplaySprite == null)
                continue;

            GameObject newWeaponDisplay = Instantiate(_weaponDisplayPrefab);
            ScalableToTargetImage newWeaponImage = newWeaponDisplay.GetComponentInChildren<ScalableToTargetImage>();
            var weaponDisplayTr = newWeaponDisplay.GetComponent<RectTransform>();
            weaponDisplayTr.SetParent(_weaponDisplayRow);
            weaponDisplayTr.localScale = Vector3.one;
            newWeaponImage.ChangeImageSprite(newCharStartingWeapon.WeaponDisplaySprite);

            InformationButton weaponInfoBtn = newWeaponDisplay.transform.GetComponentInChildren<InformationButton>();
            weaponInfoBtn.SetValues(null, newCharStartingWeapon, null);
            _displayingWeapons.Add(newWeaponDisplay.gameObject);
        }

        var displayingItemsCopy = new List<GameObject>(_displayingItems);
        foreach (var existingItem in displayingItemsCopy)
        {
            _displayingItems.Remove(existingItem);
            Destroy(existingItem);
        }

        foreach (var newCharStartingItems in newShowingChar.InitialPassives)
        {
            if (newCharStartingItems.ItemSprite == null)
                continue;
            GameObject newItemDisplay = Instantiate(_itemDisplayPrefab);
            ScalableToTargetImage newItemImage = newItemDisplay.GetComponentInChildren<ScalableToTargetImage>();
            var itemDisplayTr = newItemDisplay.GetComponent<RectTransform>();
            itemDisplayTr.SetParent(_itemDisplayRow);
            itemDisplayTr.localScale = Vector3.one;
            newItemImage.ChangeImageSprite(newCharStartingItems.ItemSprite);

            InformationButton weaponInfoBtn = newItemDisplay.transform.GetComponentInChildren<InformationButton>();
            weaponInfoBtn.SetValues(null, null, newCharStartingItems);
            _displayingItems.Add(newItemDisplay.gameObject);
        }
    }
}
