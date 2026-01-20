using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CreateSupportObjAfterAnimWeaponInteraction : WeaponInteraction
{
    new public static int maxStacks => -1;
    [SerializeField] CustomAnimation _animBeforeSpawning;
    [SerializeField] SupportObjectData _createdSupportObj;
    [SerializeField] Vector2 _offsetFromPlayer;
    [SerializeField] float _rotation;

    public override void Initialize(WeaponInteraction original, WeaponInteractionData interactionData, List<WeaponAttackManager> affectedWeapons)
    {
        base.Initialize(original, interactionData, affectedWeapons);
        var createSupportObjOriginal = original as CreateSupportObjAfterAnimWeaponInteraction;
        _animBeforeSpawning = createSupportObjOriginal._animBeforeSpawning;
        _createdSupportObj = createSupportObjOriginal._createdSupportObj;
        _offsetFromPlayer = createSupportObjOriginal._offsetFromPlayer;
        _rotation = createSupportObjOriginal._rotation;
    }

    public override void InteractionStart()
    {
        base.InteractionStart();
        CustomAnimator animatorObj = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["AnimatedObject"], PlayerControl.pc.transform.position, Quaternion.identity).GetComponent<CustomAnimator>();

        CustomAnimation animationCopy = new(animatorObj, _animBeforeSpawning);
        animationCopy.Events.Add(new(null, _animBeforeSpawning.Frames.Length - 1, CreateSupportObj));
        animationCopy.Events.Add(new(null, _animBeforeSpawning.Frames.Length - 1, () => GameObject.Destroy(animatorObj.gameObject)));

        animatorObj.AddAnimations(new (){ animationCopy });
        animatorObj.ChangeAnim(animationCopy.AnimationName);

    }

    void CreateSupportObj()
    {
        //cast a raycast from the player to the suposed spawn position, colliding only with walls. if the raycast hits a wall, create the support obj in the hit point instead
        Vector2 desiredSpawnPos = (Vector2)PlayerControl.pc.transform.position + _offsetFromPlayer;
        Vector2 spawnCheckVector = desiredSpawnPos - (Vector2)PlayerControl.pc.transform.position;
        float checkDist = spawnCheckVector.magnitude;
        Vector2 checkDir = spawnCheckVector.normalized;

        var spawnObstacle = Physics2D.Raycast(PlayerControl.pc.transform.position, checkDir, checkDist, LayerMask.NameToLayer("Map"));

        Vector2 actualSpawnPos = spawnObstacle ? spawnObstacle.point : desiredSpawnPos;

        Utility.GenerateSupportObj(_createdSupportObj, actualSpawnPos, Quaternion.Euler(0, 0, _rotation));
    }
}
