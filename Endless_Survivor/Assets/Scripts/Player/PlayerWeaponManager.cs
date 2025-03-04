using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField] GameObject _weaponPrefab;
    [SerializeField] GameObject _handPrefab;
    [SerializeField] Transform[] _gunPositions;
    [SerializeField] Transform _gunsHolder;
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

        Transform newGun = Instantiate(_weaponPrefab, handPosition, Quaternion.identity).transform;
        newGun.transform.SetParent(_gunsHolder);
        GameObject hand = Instantiate(_handPrefab, handPosition, Quaternion.identity);
        hand.GetComponent<SpriteRenderer>().sprite = GameManager.gm.selectedCharacter.CharacterHands[Random.Range(0, GameManager.gm.selectedCharacter.CharacterHands.Length)];
        hand.transform.SetParent(newGun.transform);
        _heldGuns.Add(newGun);
        weaponData.WeaponDataTransferInterface.TransferData(newGun.gameObject, weaponData);
    }
}
