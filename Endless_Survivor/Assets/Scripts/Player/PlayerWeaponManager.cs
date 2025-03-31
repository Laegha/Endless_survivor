using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField] Transform[] _gunPositions;
    [SerializeField] Transform _gunsHolder;
    [SerializeField] PlayerControl _playerControl;
    Dictionary<Weapon, Transform> _heldWeapons = new Dictionary<Weapon, Transform>();

    public void PickupWeapon(WeaponData weaponData, WeaponStats weaponStats)
    {
        if(_heldWeapons.Count == _gunPositions.Length)
        {
            GameUIManager.uiManager.WeaponOverrideMenu.DisplayMenu(_heldWeapons.Keys.ToList(), SwitchWeapon);
            return;
        }
        GenerateWeapon(weaponData, weaponStats);
    }

    void GenerateWeapon(WeaponData weaponData, WeaponStats weaponStats, Transform gunPosition = null)
    {
        if (gunPosition == null)
            gunPosition = _gunPositions[_heldWeapons.Count];
            
        Vector2 handPosition = gunPosition.position;

        Transform newWeapon = Instantiate(GameManager.gm.prefabHolder.Prefabs["Weapon"], handPosition, Quaternion.identity).transform;
        newWeapon.transform.SetParent(_gunsHolder);
        
        GameObject hand = Instantiate(GameManager.gm.prefabHolder.Prefabs["Hand"], handPosition, Quaternion.identity);
        hand.GetComponent<SpriteRenderer>().sprite = GameManager.gm.selectedCharacter.CharacterHands[Random.Range(0, GameManager.gm.selectedCharacter.CharacterHands.Length)];
        hand.transform.SetParent(newWeapon.transform);

        weaponData.WeaponDataTransferInterface.TransferData(newWeapon.gameObject, weaponData, weaponStats);
        Weapon weapon = newWeapon.GetComponent<Weapon>();
        _heldWeapons.Add(weapon, gunPosition);
        weapon.PlayerControl = _playerControl;
    }

    void SwitchWeapon(Weapon removedWeapon)
    {
        Transform gunPosition = _heldWeapons[removedWeapon];
        _heldWeapons.Remove(removedWeapon);
        Destroy(removedWeapon.gameObject);
        GenerateWeapon(GameUIManager.uiManager.WeaponPickupMenu.CurrDisplayingWeapon, GameUIManager.uiManager.WeaponPickupMenu.CurrWeaponStats, gunPosition);
    }
}
