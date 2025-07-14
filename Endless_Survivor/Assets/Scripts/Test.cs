using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] PickupData spawnedPickup;
    [SerializeField] PickupControl pickup;

    private void Start()
    {
        print(GameManager.gm.Unlockments.unlocked_characters);
        foreach(var charac in GameManager.gm.Unlockments.unlocked_weapons)
            print("CHARACTER: " + charac.Key + " IS " +  charac.Value);
    }

}
