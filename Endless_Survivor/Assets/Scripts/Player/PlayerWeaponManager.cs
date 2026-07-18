using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField] Transform _gunsHolder;
    [SerializeField] PlayerControl _playerControl;
    [SerializeField] float _weaponDistFromPlayer;
    //Dictionary<Weapon, Transform> _heldWeapons = new Dictionary<Weapon, Transform>();
    System.Action _onWeaponPickup;
    System.Action _onWeaponRemoved;
    List<WeaponHolder> _weaponHolders = new List<WeaponHolder>();
    List<WeaponHolderInfo> _totalHoldersInfos = new();

    public List<WeaponAttackManager> HeldWeapons
    {
        get
        {
            List<WeaponAttackManager> list = new List<WeaponAttackManager>();
            _weaponHolders.ForEach(weaponHolder => list.Add(weaponHolder.holdingWeapon));
            return list;
        }
    }
    public System.Action OnWeaponPickup { get  { return _onWeaponPickup; } set { _onWeaponPickup = value; } }
    public System.Action OnWeaponRemoved {  get { return _onWeaponRemoved; } set { _onWeaponRemoved = value; } }
    public Dictionary<CustomFlags.IWeaponTag, int> HeldWeaponTags
    {
        get
        {
            Dictionary<CustomFlags.IWeaponTag, int> heldTags= new();
            foreach(var heldWeapon in _weaponHolders)
            {
                if (heldWeapon.holdingWeapon == null)
                    continue;
                foreach(var tag in heldWeapon.holdingWeapon.WeaponData.WeaponTags)
                {
                    if(!heldTags.ContainsKey(tag))
                    {
                        heldTags.Add(tag, 1);
                        continue;
                    }
                    heldTags[tag]++;
                }
            }
            return heldTags;
        }
    }
    public bool CanPickWeapon
    {
        get
        {
            return _totalHoldersInfos.Count > 0;
        }
    }

    public void PickupWeapon(WeaponData weaponData, WeaponStats weaponStats)
    {
        if(_weaponHolders.Count >= _totalHoldersInfos.Count)
        {
            List<WeaponAttackManager> weapons = new List<WeaponAttackManager>();
            _weaponHolders.ForEach(x => weapons.Add(x.holdingWeapon));
            GameUIManager.uiManager.WeaponOverrideMenu.DisplayMenu(weapons, weaponData, weaponStats, SwitchWeapon);
            return;
        }
        GenerateWeapon(weaponData, weaponStats);
    }

    void GenerateWeapon(WeaponData weaponData, WeaponStats weaponStats)
    {
        //instantiate weapon
        Transform newWeapon = Instantiate(GameManager.gm.prefabHolder.Prefabs["Weapon"]).transform;
        WeaponAttackManager weaponAttackManager = newWeapon.GetComponent<WeaponAttackManager>();

        //check for empty holders
        WeaponHolder emptyHolder = _weaponHolders.Where(x => x.holdingWeapon == null).FirstOrDefault();
        if(emptyHolder != null)
        {
            newWeapon.transform.SetParent(emptyHolder.handTransform);
            newWeapon.transform.localPosition = Vector2.zero;
        }
        else
        {
            List<WeaponHolderInfo> pendingHolderInfos = _totalHoldersInfos.Where(x => !_weaponHolders.Any(y => y.holderInfo == x)).ToList();
            emptyHolder = GenerateWeaponHolder(pendingHolderInfos[Random.Range(0, pendingHolderInfos.Count)]);

            newWeapon.transform.SetParent(emptyHolder.handTransform);
            newWeapon.transform.localPosition = Vector2.zero;

        }
        emptyHolder.holdingWeapon = weaponAttackManager;

        UpdateWeaponPositions();
        weaponAttackManager.Initiate(weaponData.AttackConditions, weaponStats, weaponData);
        _onWeaponPickup?.Invoke();
    }
    void UpdateWeaponPositions()
    {
        List<WeaponHolder> updatingHolders = _weaponHolders.Where(x => !x.holderInfo.UseFixedPosition).ToList();
        int nonStaticHoldersCount = updatingHolders.Count;
        //float angleStep = 15 /** Mathf.Floor((nonStaticHoldersCount + 1) / 2) -15*/;
        float significantWeapons = Mathf.Floor((nonStaticHoldersCount - 1) / 2);
        float angleStep = nonStaticHoldersCount <= 2 ? 0 : 90 / significantWeapons;
        float startAngle = 45;

        int x = 0;
        
        foreach(var weaponHolder in updatingHolders)
        {
            float cosX = Mathf.Cos(Mathf.PI * x);
            float angleStart = 90 * cosX + startAngle * cosX;
            float angleMultiplier = cosX * Mathf.Floor(x / 2);
            float angle = angleStart - angleStep * angleMultiplier;
            Vector2 newHolderPosition = Utility.GetPointInCircle(_weaponDistFromPlayer, angle);
            weaponHolder.handTransform.localPosition = newHolderPosition;
            weaponHolder.holdingWeapon.UpdatedPosition(newHolderPosition);
            x++;
        }
    }

    void SwitchWeapon(WeaponAttackManager removedWeapon)
    {
        RemoveWeapon(removedWeapon);
        GenerateWeapon(GameUIManager.uiManager.WeaponPickupMenu.CurrDisplayingWeapon, GameUIManager.uiManager.WeaponPickupMenu.CurrWeaponStats);
    }
    public void RemoveWeapon(WeaponAttackManager removedWeapon)
    {
        var weaponHolder = _weaponHolders.Find(x => x.holdingWeapon == removedWeapon);
        DestroyImmediate(removedWeapon.gameObject);
        if (weaponHolder != null && !weaponHolder.holderInfo.Permanent)
        {
            _weaponHolders.Remove(weaponHolder);
            DestroyImmediate(weaponHolder.handTransform.gameObject);
            PlayerControl.pc.PlayerMaterialManager.CleanRenderers();
        }
        _onWeaponRemoved?.Invoke();
    }

    public void LevelUpWeapons(List<WeaponAttackManager> weapons = null)
    {
        if(weapons == null || weapons.Count == 0 )
        {
            weapons = new();
            _weaponHolders.ForEach(x => weapons.Add(x.holdingWeapon));
        }

        weapons.ForEach(x => x.InducedLevelUp());
    }
    //List<WeaponHolderInfo> addedWeaponHolders --> should know sprite (random from character, or fixed), position (part of the circle or fixed) and visibility (always visible or only if it has weapon)
    public void AddWeaponHolder(WeaponHolderInfo addedMaxWeapon)
    {
        _totalHoldersInfos.Add(addedMaxWeapon);
        if (addedMaxWeapon.Permanent)
            GenerateWeaponHolder(addedMaxWeapon);
    }
    WeaponHolder GenerateWeaponHolder(WeaponHolderInfo generatingHolder)
    {
        GameObject hand = Instantiate(GameManager.gm.prefabHolder.Prefabs["Hand"], _gunsHolder);
        var handRenderer = hand.GetComponent<SpriteRenderer>();
        handRenderer.sprite = generatingHolder.RandomHandSprite;
        PlayerControl.pc.PlayerMaterialManager.AddRenderer(handRenderer);

        Transform handTr = hand.transform;
        handTr.SetParent(_gunsHolder);
        if(generatingHolder.UseFixedPosition)
            handTr.localPosition = generatingHolder.FixedPosition;

        WeaponHolder generatedHolder = new(generatingHolder, hand.transform);
        _weaponHolders.Add(generatedHolder);
        return generatedHolder;
    }
    
}
