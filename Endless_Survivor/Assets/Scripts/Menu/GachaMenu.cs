using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GachaMenu : MonoBehaviour, IPointerDownHandler
{
    [Header("References")]
    [SerializeField] GameObject _menuObj;
    [SerializeField] TextMeshProUGUI _availableCoinsDisplay;
    [SerializeField] Image _prizeImage;
    [SerializeField] RectTransform _prizeImageTarget;
    [SerializeField] TextMeshProUGUI _prizeTypeDisplay;
    [SerializeField] TextMeshProUGUI _prizeNameDisplay;
    [SerializeField] ObjectRotatorByDrag _leverRotator;

    [Header("GraphicStates")]
    [SerializeField] List<GachaMenuGfxState> _gfxStates;
    int _currStateIndex = 0;

    Sprite _unlockedElementSprite;
    string _unlockedElementName;
    string _unlockedElementType;

    bool _skipAnimation = false;
    float _stateTimer = 0;
    bool _enoughCoinsToGacha => UnlockmentsManager.GachaCoins >= GachaUnlocker.gachaCoinCost;
    private void Start()
    {
        _leverRotator.OnLimitReached += NextState;
    }
    private void Update()
    {
        if (!_gfxStates[_currStateIndex].endsByTime)
            return;
        _stateTimer += Time.deltaTime;
        if(_stateTimer >= _gfxStates[_currStateIndex].stateDuration + _gfxStates[_currStateIndex].delayAfterEnding)
        {
            NextState();
        }
    }
    public void DisplayMenu()
    {
        _menuObj.SetActive(true);

        _availableCoinsDisplay.text = "x" + UnlockmentsManager.GachaCoins;

    }
    public void InsertCoin()
    {
        if (!_enoughCoinsToGacha)
        {
            //play sfx
            //play animation?
            print("NOT ENOUGH COINS");
            return;
        }
        var unlockedElement = GachaUnlocker.UnlockRandomElement();

        _unlockedElementName = unlockedElement.name;
        if (unlockedElement.GetType() == typeof(CharacterData))
        {
            var unlockedCharacter = (CharacterData)unlockedElement;
            _unlockedElementType = "Character";
            _unlockedElementSprite = unlockedCharacter.MenuImage;
        }
        else if (unlockedElement.GetType() == typeof(WeaponData))
        {
            var unlockedWeapon = (WeaponData)unlockedElement;
            _unlockedElementType = "Weapon";
            _unlockedElementSprite = unlockedWeapon.WeaponDisplaySprite;
        }
        else if (unlockedElement.GetType() == typeof(PassiveItemData))
        {
            var unlockedPassiveItem = (PassiveItemData)unlockedElement;
            _unlockedElementType = "Passive Item";
            _unlockedElementSprite = unlockedPassiveItem.ItemSprite;
        }

        _availableCoinsDisplay.text = "x" + UnlockmentsManager.GachaCoins;
        if(_skipAnimation)
        {
            ChangeState("OpeningPrize");
            return;
        }
        NextState();
    }
    public void ActivateLeverRotator() => _leverRotator.IsActive = true;
    public void DeactivateLeverRotator() => _leverRotator.IsActive = false;
    public void ScalePrizeImage() => Utility.ScaleImageToFitTarget(_prizeImage.GetComponent<RectTransform>(), _unlockedElementSprite, _prizeImageTarget.sizeDelta);
    public void DisplayPrizeData()
    {
        _prizeImage.sprite = _unlockedElementSprite;
        _prizeNameDisplay.text = _unlockedElementName;
        _prizeTypeDisplay.text = _unlockedElementType;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
            print("Pointer detected");
        if(_gfxStates[_currStateIndex].isSkipable)
        {
            print("Should skip state");
            SkipCurrentState();
        }
    }
    void ChangeState(GachaMenuGfxState state)
    {
        state.StartState();
        _currStateIndex = _gfxStates.IndexOf(state);
    }
    void ChangeState(string stateName)
    {
        ChangeState(_gfxStates.Find(x => x.stateName == stateName));
    }
    void ChangeState(int stateIndex)
    {
        ChangeState(_gfxStates[stateIndex]);
    }
    void SkipCurrentState()
    {
        _gfxStates[_currStateIndex].SkipState();
        NextState();
    }
    public void NextState()
    {
        _stateTimer = 0;
        ChangeState(_currStateIndex < _gfxStates.Count - 1 ? _currStateIndex + 1 : 0);
    }
}
