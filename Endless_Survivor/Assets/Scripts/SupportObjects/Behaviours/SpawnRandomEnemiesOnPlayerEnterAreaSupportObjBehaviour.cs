using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnRandomEnemiesOnPlayerEnterAreaSupportObjBehaviour : UseAreaAroundSupportObjBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] List<RouletteElementChance<EnemyData>> _possibleEnemies;
    [SerializeField] RandomBetweenTwoConstants _ammount;
    [SerializeReference] IPattern _creationPattern;
    [SerializeField] CustomAnimation _spawnAnimation;
    [SerializeField] int _spawnFrame;
    [SerializeField] bool _destroyAfterSpawn;

    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var spawnEnemiesOriginal = original as SpawnRandomEnemiesOnPlayerEnterAreaSupportObjBehaviour;
        _possibleEnemies = new(spawnEnemiesOriginal._possibleEnemies);
        _ammount = spawnEnemiesOriginal._ammount;
        _creationPattern = spawnEnemiesOriginal._creationPattern;
        
        _spawnAnimation = new(ObjControl.Animator, spawnEnemiesOriginal._spawnAnimation);
        ObjControl.Animator.AddAnimations(new() { _spawnAnimation });

        _spawnFrame = spawnEnemiesOriginal._spawnFrame;
        _destroyAfterSpawn = spawnEnemiesOriginal._destroyAfterSpawn;

        OnObjEnterArea += CheckObj;
    }
    void CheckObj(GameObject obj)
    {
        if (obj.transform.root != PlayerControl.pc.transform)
            return;

        PlaySpawnAnim();
    }
    void PlaySpawnAnim()
    {
        ObjControl.Animator.ChangeAnim(_spawnAnimation.AnimationName);
        float frameDelay = _spawnFrame / _spawnAnimation.FramesPerSecond;
        GameManager.gm.DelayAction(frameDelay, SpawnEnemies, () => ObjControl == null);
    }
    void SpawnEnemies()
    {
        int spawnedAmmount = (int)_ammount.rand;
        var spawnedEnemiesPositions = _creationPattern.GetPositions(ObjControl.transform.position, spawnedAmmount).ToList();
        for (int i = 0; i < spawnedAmmount; i++)
        {
            EnemyData generatedEnemy = Utility.GetRouletteElement(_possibleEnemies);
            Vector2 tilePos = new((int)spawnedEnemiesPositions[i].x, (int)spawnedEnemiesPositions[i].y);
            if (!MapManager.mm.GenerationHandler.TileMatrix.ContainsKey(tilePos))
                continue;
            var tilesInPos = MapManager.mm.GenerationHandler.TileMatrix[tilePos];
            if (tilesInPos[0].IsWall || !tilesInPos[0].IsLoaded)
                continue;
            var generatingTile = tilesInPos[0];
            EnemySpawnManager.esm.SpawnEnemy(generatingTile, generatedEnemy);

        }
        if (_destroyAfterSpawn)
            ObjectDestroyingManager.odm.DestroyObj(ObjControl.BehaviourManager);
    }
}
