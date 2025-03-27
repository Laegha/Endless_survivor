using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        CharacterData selectedChar = GameManager.gm.selectedCharacter;
        GameObject player = Instantiate(GameManager.gm.prefabHolder.Prefabs["Character"], transform.position, Quaternion.identity);
        GameManager.gm.player = player.transform;
        
        PlayerWeaponManager playerWeaponManager = player.GetComponent<PlayerWeaponManager>();
        PlayerControl playerControl = player.GetComponent<PlayerControl>();
        playerControl.PlayerAnimator.AddAnimations(selectedChar.Animations);
        playerControl.PlayerStats = new PlayerStats(selectedChar.PlayerStats);
        
        //generate initial weapons and passives
        foreach(WeaponData weaponData in selectedChar.InitialWeapons)
        {
            playerWeaponManager.PickupWeapon(weaponData, new WeaponStats(weaponData.WeaponStats));

        }

    }
}
