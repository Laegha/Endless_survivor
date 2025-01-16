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
        Vector2 gunPosition = _gunPositions[_heldGuns.Count].position;
        Transform newGun = Instantiate(gunPrefab, gunPosition, Quaternion.identity).transform;
        newGun.SetParent(transform);
    }
}
