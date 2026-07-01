using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        CharacterData selectedChar = GameManager.gm.selectedCharacter;
        GameObject player = Instantiate(GameManager.gm.prefabHolder.Prefabs["Character"], transform.position, Quaternion.identity);
        
        PlayerControl playerControl = player.GetComponent<PlayerControl>();
        playerControl.CharacterData = selectedChar;
        playerControl.PlayerAnimator.AddAnimations(selectedChar.Animations);
        playerControl.CostumeManager.CostumeSettings = selectedChar.CostumeSettings;
        playerControl.PlayerStats = new PlayerStats(selectedChar.PlayerStats);
        playerControl.PlayerHPManager.OnHitSound= selectedChar.OnHitSound;
        playerControl.PlayerHPManager.OnDeathSound = selectedChar.OnDeathSound;
        foreach(var weaponHolderInfo in selectedChar.InitialWeaponHolders)
        {
            playerControl.WeaponManager.AddWeaponHolder(weaponHolderInfo);

        }
        CapsuleCollider2D playerCollider = player.GetComponent<CapsuleCollider2D>();
        playerCollider.offset = Vector2.zero;
        //playerCollider.offset = selectedChar.ColliderOffset;

        playerControl.ChangeCollider(selectedChar.ColliderDirection, selectedChar.ColliderSize, selectedChar.ColliderOffset);

        //generate initial weapons and passives
        foreach(WeaponData weaponData in selectedChar.InitialWeapons)
        {
            var weaponStats = new WeaponStats(weaponData.WeaponStats);
            weaponStats.SetTrueLevelStats(weaponData.StatsIncreaseScale, 1);
            playerControl.WeaponManager.PickupWeapon(weaponData, weaponStats);

        }
        foreach(var passiveItemData in selectedChar.InitialPassives)
        {
            playerControl.PassiveItemManager.AddPassiveItem(passiveItemData);
        }

    }
}
