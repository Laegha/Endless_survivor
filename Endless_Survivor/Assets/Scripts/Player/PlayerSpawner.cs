using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        Instantiate(GameManager.gm.selectedCharacter.CharacterPrefab, transform.position, Quaternion.identity);
    }
}
