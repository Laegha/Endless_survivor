using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField] Transform[] _gunPositions;
    Dictionary <int, Transform> _heldGuns;

    public void PickupGun(GameObject gunPrefab)
    {
        if(_heldGuns.Count == _gunPositions.Length) 
            return;
        GenerateGun(gunPrefab);
    }

    void GenerateGun(GameObject gunPrefab)
    {
        GameObject hand = Instantiate(GameManager.gm.selectedCharacter.CharacterHands[Random.Range(0, GameManager.gm.selectedCharacter.CharacterHands.Length)]);

        Vector2 handPosition = _gunPositions[_heldGuns.Count].position;
        Transform newGun = Instantiate(hand, handPosition, Quaternion.identity).transform;
        newGun.SetParent(hand.transform);
    }
}
