using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSupportObjectsItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] GenericNumHolder<SupportObjectData>[] _spawnedObjects;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var spawnSupportOriginal = original as SpawnSupportObjectsItemBehaviour;
        _spawnedObjects = spawnSupportOriginal._spawnedObjects;
        behaviourManager.onPicked += SpawnSupportObjects;
    }

    void SpawnSupportObjects()
    {
        foreach(var obj in _spawnedObjects )
        {
            for(int i = 0; i < obj.num; i++)
            {
                var spawnedObj = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["SupportObject"]).GetComponent<SupportObjectControl>();
                obj.generic.TransferData(spawnedObj);
            }
        }
    }
}
