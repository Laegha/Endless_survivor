using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        GameObject player = Instantiate(GameManager.gm.selectedCharacter.CharacterPrefab, transform.position, Quaternion.identity);
        GameManager.gm.player = player.transform;
        //generate initial weapons and passives
    }
}
