using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] GameObject _menuBG;
    [SerializeField] WeaponPickupMenu _weaponPickupMenu;
    [SerializeField] WeaponSwitchMenu _weaponOverrideMenu;
    [SerializeField] PassiveItemPickupMenu _passiveItemPickupMenu;
    [SerializeField] PlayerHPBar _playerHPBar;
    [SerializeField] IntensityUI _intensityUI;
    [SerializeField] UIPointerManager _pointerManager;
    [SerializeField] GameObject _messageHolder;

    [InspectorLabel("Information menus")]
    [SerializeField] CharacterInfoMenu _charInfoMenu;
    [SerializeField] WeaponInfoMenu _weaponInfoMenu;
    [SerializeField] PassiveItemInfoMenu _itemInfoMenu;

    TimescaleChangeInfo _currPauseTimescaleChange;

    public static GameUIManager instance;
    public static GameUIManager uiManager {  get { return instance; }}
    public WeaponPickupMenu WeaponPickupMenu { get { return _weaponPickupMenu; }}
    public WeaponSwitchMenu WeaponOverrideMenu { get { return _weaponOverrideMenu; }}
    public PassiveItemPickupMenu PassiveItemPickupMenu { get { return _passiveItemPickupMenu; }}
    public PlayerHPBar PlayerHPBar { get { return _playerHPBar; }}
    public IntensityUI IntensityUI { get { return _intensityUI; }}
    public UIPointerManager PointerManager { get { return _pointerManager; }}
    void Awake()
    {
        instance = this;
    }
    public void MenuDisplayed()
    {
        _menuBG.SetActive(true);
        _currPauseTimescaleChange = TimescaleManager.tm.AddTimescaleChange(new(0, false, 0));
    }
    public void MenuHid()
    {
        _menuBG.SetActive(false);
        TimescaleManager.tm.RemoveTimescaleChange(_currPauseTimescaleChange);
    }
    public void DisplayUIMessage(UIMessageInfo messageInfo)
    {
        GameObject messageGO = Instantiate(GameManager.gm.prefabHolder.Prefabs["UIMessage"], messageInfo.DrawOnTop ? transform.root : _messageHolder.transform);
        if (messageInfo.DrawOnTop)
            messageGO.transform.SetAsLastSibling();
        var messageTr = messageGO.GetComponent<RectTransform>();
        messageTr.anchoredPosition = new(0, messageInfo.MessageYPosition);
        Animator messageAn = messageGO.GetComponent<Animator>();
        messageAn.runtimeAnimatorController = messageInfo.MessageAnimator;
        messageAn.updateMode = messageInfo.AnUpdateMode;
        GameManager.gm.DelayAction(messageInfo.MessageExitDelay, () => messageAn.Play("Exit"), () => messageGO == null);
        TextMeshProUGUI messageText = messageGO.GetComponentInChildren<TextMeshProUGUI>();
        messageInfo.ApplyToText(messageText);

    }
    public void DisplayCharacterInfo(CharacterData displayingChar)
    {
        _charInfoMenu.gameObject.SetActive(true);
        _charInfoMenu.UpdateInfo(displayingChar);
    }
    public void DisplayWeaponInfo(WeaponData displayingWeapon)
    {
        _weaponInfoMenu.gameObject.SetActive(true);
        _weaponInfoMenu.UpdateInfo(displayingWeapon);
    }
    public void DisplayItemInfo(PassiveItemData displayingItem)
    {
        _itemInfoMenu.gameObject.SetActive(true);
        _itemInfoMenu.UpdateInfo(displayingItem);
    }
}
