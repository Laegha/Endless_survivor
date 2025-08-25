using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        CharacterData selectedChar = GameManager.gm.selectedCharacter;
        GameObject player = Instantiate(GameManager.gm.prefabHolder.Prefabs["Character"], transform.position, Quaternion.identity);
        
        PlayerWeaponManager playerWeaponManager = player.GetComponent<PlayerWeaponManager>();
        PlayerControl playerControl = player.GetComponent<PlayerControl>();
        playerControl.PlayerAnimator.AddAnimations(selectedChar.Animations);
        playerControl.PlayerStats = new PlayerStats(selectedChar.PlayerStats);
        playerControl.PlayerHPManager.OnHitSound= selectedChar.OnHitSound;
        playerControl.PlayerHPManager.OnDeathSound = selectedChar.OnDeathSound;
        playerWeaponManager.MaxWeapons = selectedChar.InitialMaxWeapons;
        CapsuleCollider2D playerCollider = player.GetComponent<CapsuleCollider2D>();
        playerCollider.size = selectedChar.ColliderSize;
        playerCollider.offset = selectedChar.ColliderOffset;
        playerCollider.direction = selectedChar.ColliderDirection;
        //generate initial weapons and passives
        foreach(WeaponData weaponData in selectedChar.InitialWeapons)
        {
            var weaponStats = new WeaponStats(weaponData.WeaponStats);
            weaponStats.SetTrueLevelStats(weaponData.StatsIncreaseScale, 1);
            playerWeaponManager.PickupWeapon(weaponData, weaponStats);

        }

    }
}
