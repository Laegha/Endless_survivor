using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CreateRandomEnemiesOnDestroySupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] List<RouletteElementChance<EnemyData>> _possibleEnemies;
    [SerializeField] RandomBetweenTwoConstants _ammount;
    [SerializeReference] IPattern _creationPattern;
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var createEnemiesOriginal = original as CreateRandomEnemiesOnDestroySupportObjBehaviour;
        _possibleEnemies = new(createEnemiesOriginal._possibleEnemies);
        _ammount = createEnemiesOriginal._ammount;
        _creationPattern = createEnemiesOriginal._creationPattern;
        OnDestroyed += CreateRandomEnemies;
    }

    void CreateRandomEnemies()
    {
        int createdAmmount = (int)_ammount.rand;
        var createdEnemiesPositions = _creationPattern.GetPositions(ObjControl.transform.position, createdAmmount).ToList();
        for (int i = 0; i < createdAmmount; i++)
        {
            EnemyData generatedEnemy = Utility.GetRouletteElement(_possibleEnemies);
            Vector2 tilePos = new((int)createdEnemiesPositions[i].x, (int)createdEnemiesPositions[i].y);
            if (!MapManager.mm.GenerationHandler.TileMatrix.ContainsKey(tilePos))
                continue;
            var tilesInPos = MapManager.mm.GenerationHandler.TileMatrix[tilePos];
            if (tilesInPos[0].IsWall)
                continue;
            var generatingTile = tilesInPos[0];
            EnemySpawnManager.esm.SpawnEnemy(generatingTile, generatedEnemy);

        }

    }
}
