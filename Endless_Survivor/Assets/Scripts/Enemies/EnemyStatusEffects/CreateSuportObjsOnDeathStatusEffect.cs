using UnityEngine;

public class CreateSuportObjsOnDeathStatusEffect : EnemyStatusEffect
{
    new public static bool isUsable => true;
    [SerializeField] GenericAmmountHolder<SupportObjectData>[] _createdObjsDatas;
    public override void Initialize(EnemyControl affectedEnemyControl, EnemyStatusEffect original)
    {
        base.Initialize(affectedEnemyControl, original);
        var createSupportObjOriginal = original as CreateSuportObjsOnDeathStatusEffect;
        _createdObjsDatas = createSupportObjOriginal._createdObjsDatas;
    }
    public override void EnemyKilled()
    {
        base.EnemyKilled();

        foreach(var obj in _createdObjsDatas)
        {
            for(int i = 0; i < obj.ammount; i ++)
            {
                var spawnedObj = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["SupportObject"], AffectedEnemyControl.transform.position, Quaternion.identity).GetComponent<SupportObjectControl>();
                obj.generic.TransferData(spawnedObj);
            }

        }
    }
}
