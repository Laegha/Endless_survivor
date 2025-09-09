using Microsoft.Win32.SafeHandles;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GachaMenu : MonoBehaviour, IPointerDownHandler
{
    [Header("References")]
    [SerializeField] GameObject _menuObj;
    [SerializeField] TextMeshProUGUI _availableCoinsDisplay;
    [SerializeField] GameObject _skipAnimationCheckboxFill;
    [SerializeField] Button _returnButton;
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

    bool _enoughCoinsToGacha = false;
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

        UnlockmentsManager.GetGachaCoins((int coins) => _availableCoinsDisplay.text = "x " +  coins);
    }
    public void ToggleSkipAnimation()
    {
        _skipAnimation = !_skipAnimation;
        _skipAnimationCheckboxFill.SetActive(_skipAnimation);
    }
    public void TurnOnReturnButton() => _returnButton.interactable = true;
    public async void InsertCoin()
    {
        UnlockmentsManager.GetGachaCoins((int coins) => _enoughCoinsToGacha = coins > GachaUnlocker.gachaCoinCost);
        if (!_enoughCoinsToGacha)
        {
            //play sfx
            //play animation?
            print("NOT ENOUGH COINS");
            return;
        }
        _returnButton.interactable = false;
        var unlockedElement = await GachaUnlocker.UnlockRandomElement();
        if(unlockedElement == null)
        {
            var characters = await UnlockmentsManager.UnlockedCharacters();
            var weapons = await UnlockmentsManager.UnlockedWeapons();
            var passiveItems = await UnlockmentsManager.UnlockedPassiveItems();
            List<ScriptableObject> elements = new List<ScriptableObject>();
            elements.AddRange(characters);
            elements.AddRange(weapons);
            elements.AddRange(passiveItems);
            unlockedElement = elements[Random.Range(0, elements.Count)];
        }

        if (unlockedElement.GetType() == typeof(CharacterData))
        {
            var unlockedCharacter = (CharacterData)unlockedElement;
            _unlockedElementType = "Character";
            _unlockedElementName = unlockedCharacter.CharacterName;
            _unlockedElementSprite = unlockedCharacter.MenuImage;
        }
        else if (unlockedElement.GetType() == typeof(WeaponData))
        {
            var unlockedWeapon = (WeaponData)unlockedElement;
            _unlockedElementType = "Weapon";
            _unlockedElementName = unlockedWeapon.WeaponName;
            _unlockedElementSprite = unlockedWeapon.WeaponDisplaySprite;
        }
        else if (unlockedElement.GetType() == typeof(PassiveItemData))
        {
            var unlockedPassiveItem = (PassiveItemData)unlockedElement;
            _unlockedElementType = "Passive Item";
            _unlockedElementName = unlockedPassiveItem.ItemName;
            _unlockedElementSprite = unlockedPassiveItem.ItemSprite;
        }
        UnlockmentsManager.GetGachaCoins((int coins) =>
        {
            _enoughCoinsToGacha = coins > GachaUnlocker.gachaCoinCost;
            _availableCoinsDisplay.text = "x" + coins;
        });
        if(_skipAnimation)
        {
            SkipStateUntil("OpeningPrize");
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
    void SkipStateUntil(string targetStateName)
    {
        for(int i = 0; i < _gfxStates.Count; i++)
        {
            if (_gfxStates[i].stateName == targetStateName)
                return;

            ChangeState(i);
            SkipCurrentState();
        }
    }
}
