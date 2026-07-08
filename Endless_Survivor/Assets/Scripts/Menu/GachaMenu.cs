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
    [SerializeField] Animator _catAnimator;
    [SerializeField] List<RouletteElementChance<RuntimeAnimatorController>> _catAnimatorControllers;
    [SerializeField] Image[] _gachaBallRenderers;
    [SerializeField] List<RouletteElementChance<Color>> _gachaBallColors;
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
    bool _isUsing;
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
        if(_catAnimator != null)
            _catAnimator.runtimeAnimatorController = Utility.GetRouletteElement(_catAnimatorControllers);
        
        UnlockmentsManager.GetGachaCoins((int coins) => _availableCoinsDisplay.text = "x " +  coins);
    }
    public void ToggleSkipAnimation()
    {
        _skipAnimation = !_skipAnimation;
        _skipAnimationCheckboxFill.SetActive(_skipAnimation);
    }
    public void TurnOnReturnButton() => _returnButton.interactable = true;
    public void InsertCoin()
    {
        if (_isUsing)
            return;
        UnlockmentsManager.GetGachaCoins((int coins) => _enoughCoinsToGacha = coins > GachaUnlocker.gachaCoinCost);
        if (!_enoughCoinsToGacha)
        {
            //play sfx
            //play animation?
            print("NOT ENOUGH COINS");
            return;
        }
        _isUsing = true;
        _returnButton.interactable = false;
        var unlockedElement = GachaUnlocker.UnlockRandomElement();
        if(unlockedElement == null)
        {
            var characters = GameManager.gm.UnlockedElementHelper.UnlockedCharacters;
            var weapons = GameManager.gm.UnlockedElementHelper.UnlockedWeapons;
            var passiveItems = GameManager.gm.UnlockedElementHelper.UnlockedPassiveItems;
            List<ScriptableObject> elements = new List<ScriptableObject>();
            foreach (var character in characters)
                elements.Add(character.element);
            foreach (var weapon in weapons)
                elements.Add(weapon.element);
            foreach (var passive in passiveItems)
                elements.Add(passive.element);
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
        Color ballColor = Utility.GetRouletteElement(_gachaBallColors);
        foreach (var ballRenderer in _gachaBallRenderers)
        {
            ballRenderer.color = ballColor;
        }
        if (_skipAnimation)
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
            print("CURRENT STATE " + _gfxStates[_currStateIndex].stateName + " IS SKIPABLE " + _gfxStates[_currStateIndex].isSkipable);
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
    public void StopUsingGacha() => _isUsing = false;
}
