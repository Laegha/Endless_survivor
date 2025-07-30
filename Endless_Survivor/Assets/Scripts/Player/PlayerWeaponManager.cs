using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField] Transform _gunsHolder;
    [SerializeField] PlayerControl _playerControl;
    [SerializeField] float _weaponDistFromPlayer;
    //Dictionary<Weapon, Transform> _heldWeapons = new Dictionary<Weapon, Transform>();
    List<WeaponHolder> _heldWeapons = new List<WeaponHolder>();
    int _maxWeapons;

    public int MaxWeapons { set { _maxWeapons = value; } }
    public Dictionary<CustomFlags.IWeaponTag, int> HeldWeaponTags
    {
        get
        {
            Dictionary< CustomFlags.IWeaponTag, int> heldTags= new();
            foreach(var heldWeapon in _heldWeapons)
            {
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

    public void PickupWeapon(WeaponData weaponData, WeaponStats weaponStats)
    {
        if(_heldWeapons.Count >= _maxWeapons)
        {
            List<Weapon> weapons = new List<Weapon>();
            _heldWeapons.ForEach(x => weapons.Add(x.holdingWeapon));
            GameUIManager.uiManager.WeaponOverrideMenu.DisplayMenu(weapons, SwitchWeapon);
            return;
        }
        GenerateWeapon(weaponData, weaponStats);
    }

    void GenerateWeapon(WeaponData weaponData, WeaponStats weaponStats)
    {
        //instantiate weapon
        Transform newWeapon = Instantiate(GameManager.gm.prefabHolder.Prefabs["Weapon"]).transform;
        weaponData.WeaponDataTransferInterface.TransferData(newWeapon.gameObject, weaponData, weaponStats);
        Weapon weapon = newWeapon.GetComponent<Weapon>();
        weapon.PlayerControl = _playerControl;

        //check for empty holders
        WeaponHolder emptyHolder = _heldWeapons.Where(x => x.holdingWeapon == null).FirstOrDefault();
        if(emptyHolder != null)
        {
            newWeapon.transform.SetParent(emptyHolder.handTransform);
            newWeapon.transform.localPosition = Vector2.zero;
            return;
        }
        emptyHolder = new WeaponHolder();
        _heldWeapons.Add(emptyHolder);
        emptyHolder.destructible = true;

        GameObject hand = Instantiate(GameManager.gm.prefabHolder.Prefabs["Hand"], _gunsHolder);
        var handRenderer = hand.GetComponent<SpriteRenderer>();
        handRenderer.sprite = GameManager.gm.selectedCharacter.CharacterHands[Random.Range(0, GameManager.gm.selectedCharacter.CharacterHands.Length)];
        PlayerControl.pc.PlayerMaterialManager.AddRenderer(handRenderer);
        newWeapon.transform.SetParent(hand.transform);
        newWeapon.transform.localPosition = Vector2.zero;
        emptyHolder.handTransform = hand.transform;
        emptyHolder.positionStays = false;
        emptyHolder.holdingWeapon = weapon;
        UpdateWeaponPositions();
    }
    void UpdateWeaponPositions()
    {
        List<WeaponHolder> updatingHolders = _heldWeapons.Where(x => !x.positionStays).ToList();
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
            x++;
        }
    }

    void SwitchWeapon(Weapon removedWeapon)
    {
        var weaponHolder = _heldWeapons.Find(x => x.holdingWeapon == removedWeapon);
        Destroy(removedWeapon.gameObject);
        if(weaponHolder != null && weaponHolder.destructible)
        {
            DestroyImmediate(weaponHolder.handTransform.gameObject);
            _heldWeapons.Remove(weaponHolder);
        }
        PlayerControl.pc.PlayerMaterialManager.CleanRenderers();
        GenerateWeapon(GameUIManager.uiManager.WeaponPickupMenu.CurrDisplayingWeapon, GameUIManager.uiManager.WeaponPickupMenu.CurrWeaponStats);
    }
}
