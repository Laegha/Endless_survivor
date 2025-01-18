using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField] Transform[] _gunPositions;
    [SerializeField] Transform _gunsHolder;
    List<Transform> _heldGuns = new List<Transform>();

    public void PickupGun(GameObject gunPrefab)
    {
        if(_heldGuns.Count == _gunPositions.Length) 
            return;
        GenerateGun(gunPrefab);
    }

    void GenerateGun(GameObject gunPrefab)
    {
        Vector2 handPosition = _gunPositions[_heldGuns.Count].position;

        GameObject hand = Instantiate(GameManager.gm.selectedCharacter.CharacterHands[Random.Range(0, GameManager.gm.selectedCharacter.CharacterHands.Length)], handPosition, Quaternion.identity);
        hand.transform.SetParent(_gunsHolder);
        Transform newGun = Instantiate(hand, handPosition, Quaternion.identity).transform;
        newGun.SetParent(hand.transform);
        _heldGuns.Add(newGun);
    }
}
