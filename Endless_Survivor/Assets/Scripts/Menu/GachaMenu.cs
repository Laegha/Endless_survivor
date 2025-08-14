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
    [SerializeField] GachaMenuGfxHolder _gfxHolder; 

    [Header("Auxiliar")]
    [SerializeField] float _delayAfterSpinningLever = .5f;
    [SerializeField] float _delayAfterBallFall = .5f;

    [Header("GraphicStates")]
    [SerializeField] List<GachaMenuGfxState> _gfxStates;
    int _currStateIndex = 0;

    Sprite _unlockedElementSprite;
    string _unlockedElementName;
    string _unlockedElementType;

    bool _skipAnimation = false;
    bool _enoughCoinsToGacha => UnlockmentsManager.GachaCoins >= GachaUnlocker.gachaCoinCost;
    private void Start()
    {
        _gfxHolder.leverRotator.OnLimitReached += LeverSpun;
    }
    public void DisplayMenu()
    {
        _menuObj.SetActive(true);

        _gfxHolder.coinSlotOutline.SetActive(_enoughCoinsToGacha);
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
            StartCoroutine(ShowPrize());
            return;
        }
        _gfxHolder.coinSlotOutline.SetActive(false);
        _gfxHolder.baseAnimator.Play("BaseInsertCoin");
        StartCoroutine(WaitForInsertCoinAnimation());
    }
    IEnumerator WaitForInsertCoinAnimation()
    {
        yield return new WaitForSeconds(Utility.GetClipFromAnimator(_gfxHolder.baseAnimator, "BaseInsertCoin").length);
        _gfxHolder.faceAnimator.Play("FaceSpinning");
        _gfxHolder.leverOutline.SetActive(true);
        _gfxHolder.leverRotator.IsActive = true;
    }
    public void LeverSpun()
    {
        _gfxHolder.faceAnimator.Play("FaceGaching");
        _gfxHolder.leverOutline.SetActive(false);
        _gfxHolder.leverRotator.IsActive = false;
        StartCoroutine(WaitBeforeGachaBallAnimation());
    }
    IEnumerator WaitBeforeGachaBallAnimation()
    {
        yield return new WaitForSeconds(_delayAfterSpinningLever);
        _gfxHolder.ballAnimator.Play("BallFall");
        yield return new WaitForSeconds(Utility.GetClipFromAnimator(_gfxHolder.ballAnimator, "BallFall").length);
        _gfxHolder.faceAnimator.Play("FaceHappy");
        yield return new WaitForSeconds(_delayAfterBallFall);

        StartCoroutine(ShowPrize());
    }
    IEnumerator ShowPrize()
    {
        //toggle black bg
        _gfxHolder.prizeBG.SetActive(true);
        //play big ball animation
        _gfxHolder.ballPrizeAnimator.Play("PrizeSlideIn");
        yield return new WaitForSeconds(Utility.GetClipFromAnimator(_gfxHolder.ballPrizeAnimator, "PrizeSlideIn").length); //change for the duration of the ball sliding in animation
        //when it's finished, show earned prize
        _gfxHolder.prizeImage.gameObject.SetActive(true);
        Utility.ScaleImageToFitTarget(_gfxHolder.prizeImage.GetComponent<RectTransform>(), _unlockedElementSprite, _gfxHolder.prizeImageTarget.sizeDelta);
        yield return new WaitForSeconds(Utility.GetClipFromAnimator(_gfxHolder.ballPrizeAnimator, "PrizeOpen").length); //change for the duration of the ball sliding in animation
        _gfxHolder.prizeInfoAnimator.Play("InfoFadeIn");
    }
    void ClosePrizeMenu()
    {
        _gfxHolder.prizeBG.SetActive(false);
        _gfxHolder.prizeImage.gameObject.SetActive(false);
        _gfxHolder.prizeInfoAnimator.Play("Idle");
        
    }
}
