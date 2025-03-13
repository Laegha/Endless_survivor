using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField] Transform[] _gunPositions;
    [SerializeField] Transform _gunsHolder;
    [SerializeField] PlayerControl _playerControl;
    List<Transform> _heldGuns = new List<Transform>();

    public void PickupGun(WeaponData weaponData)
    {
        if(_heldGuns.Count == _gunPositions.Length) 
            return;
        GenerateGun(weaponData);
    }

    void GenerateGun(WeaponData weaponData)
    {
        Vector2 handPosition = _gunPositions[_heldGuns.Count].position;

        Transform newWeapon = Instantiate(GameManager.gm.prefabHolder.Prefabs["Weapon"], handPosition, Quaternion.identity).transform;
        newWeapon.transform.SetParent(_gunsHolder);
        GameObject hand = Instantiate(GameManager.gm.prefabHolder.Prefabs["Hand"], handPosition, Quaternion.identity);
        hand.GetComponent<SpriteRenderer>().sprite = GameManager.gm.selectedCharacter.CharacterHands[Random.Range(0, GameManager.gm.selectedCharacter.CharacterHands.Length)];
        hand.transform.SetParent(newWeapon.transform);
        _heldGuns.Add(newWeapon);
        weaponData.WeaponDataTransferInterface.TransferData(newWeapon.gameObject, weaponData);
        newWeapon.GetComponent<Weapon>().PlayerControl = _playerControl;
    }
}
