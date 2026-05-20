using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSupportObjectsItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] GenericNumHolder<SupportObjectData>[] _spawnedObjects;
    List<GameObject> _generatedSupportObjs = new List<GameObject>();
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
                _generatedSupportObjs.Add(spawnedObj.transform.root.gameObject);
                obj.generic.TransferData(spawnedObj);
            }
        }
    }
    public override void RemoveBehaviour()
    {
        foreach(var obj in _generatedSupportObjs)
        {
            if (obj == null)
                continue;
            GameObject.Destroy(obj);
        }
    }
}
