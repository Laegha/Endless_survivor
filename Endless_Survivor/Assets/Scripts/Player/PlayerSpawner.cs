using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        CharacterData selectedChar = GameManager.gm.selectedCharacter;
        GameObject player = Instantiate(selectedChar.CharacterPrefab, transform.position, Quaternion.identity);
        GameManager.gm.player = player.transform;
        
        PlayerWeaponManager playerWeaponManager = player.GetComponent<PlayerWeaponManager>();
        
        //generate initial weapons and passives
        foreach(WeaponData weaponData in selectedChar.InitialWeapons)
        {
            playerWeaponManager.PickupGun(weaponData);

        }

    }
}
