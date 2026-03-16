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
        playerControl.PlayerAnimator.AddAnimations(selectedChar.Animations);
        playerControl.PlayerStats = new PlayerStats(selectedChar.PlayerStats);
        playerControl.PlayerHPManager.OnHitSound= selectedChar.OnHitSound;
        playerControl.PlayerHPManager.OnDeathSound = selectedChar.OnDeathSound;
        playerControl.WeaponManager.MaxWeapons = selectedChar.InitialMaxWeapons;
        CapsuleCollider2D playerCollider = player.GetComponent<CapsuleCollider2D>();
        playerCollider.size = selectedChar.ColliderSize;
        playerCollider.offset = Vector2.zero;
        //playerCollider.offset = selectedChar.ColliderOffset;
        playerCollider.direction = selectedChar.ColliderDirection;

        foreach(Transform child in playerControl.transform.GetComponentInChildren<Transform>())
        {
            if(child == playerCollider.transform) 
                continue;
            child.localPosition -= new Vector3(selectedChar.ColliderOffset.x, selectedChar.ColliderOffset.y, 0);
        }

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
