using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class TeleportToNearestEnemySupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 1;

    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        OnStart += Teleport;
    }
    void Teleport()
    {
        List<GameObject> closestEnemies = new(EnemySpawnManager.esm.Enemies);
        closestEnemies.Sort((enemy1, enemy2) => Vector3.Distance(enemy1.transform.position, ObjControl.transform.position).CompareTo(Vector3.Distance(enemy2.transform.position, ObjControl.transform.position)));

        ObjControl.transform.position = closestEnemies[0].transform.position;
    }

}
