using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CreateSupportObjsOnPlayerPosOverTimePlayerEffect : PlayerStatusEffect
{
    new public static int maxStacks => -1;

    [SerializeField] List<SupportObjectData> _createdSupportObjs;
    [SerializeReference] IPattern _creationPattern;
    [SerializeField] bool _createOnStart;
    [SerializeField] RandomBetweenTwoConstants _timeBetweenCreations;
    [Tooltip("If max is <0, then this doesn't end the effect")][SerializeField] RandomBetweenTwoConstants _createdObjsTillEffectEnd;

    float _timer;
    float _currCreationTime;
    int _creationsToEnd;
    int _objsCreated;
    public override void Initialize(PlayerStatusEffect original)
    {
        base.Initialize(original);
        var createSupportObjsOriginal = original as CreateSupportObjsOnPlayerPosOverTimePlayerEffect;
        _createdSupportObjs = createSupportObjsOriginal._createdSupportObjs;
        _creationPattern = createSupportObjsOriginal._creationPattern;
        _createOnStart = createSupportObjsOriginal._createOnStart;
        _timeBetweenCreations = createSupportObjsOriginal._timeBetweenCreations;
        _createdObjsTillEffectEnd = createSupportObjsOriginal._createdObjsTillEffectEnd;

        if (_createdObjsTillEffectEnd.max > 0)
            _creationsToEnd = (int)_createdObjsTillEffectEnd.rand;
        else
            _creationsToEnd = -1;
        _currCreationTime = _timeBetweenCreations.rand;
    }

    public override void Start()
    {
        base.Start();
        if (_createOnStart)
            CreateSupportObjs();
    }

    public override void Update()
    {
        base.Update();
        _timer += Time.deltaTime;
        if (_timer < _currCreationTime)
            return;
        CreateSupportObjs();
        _timer = 0;
        _currCreationTime = _timeBetweenCreations.rand;
        if (_creationsToEnd < 0)
            return;

        _objsCreated++;
        
        if (_objsCreated < _creationsToEnd)
            return;

        Debug.Log("SHOULD END THE EFEFCT0");
        PlayerControl.pc.StatusEffectManager.RemoveEffect(this);
    }

    void CreateSupportObjs()
    {
        var shuffledObjs = Utility.ShuffleList(_createdSupportObjs);
        List<Vector2> createdObjsPositions = _creationPattern.GetPositions(PlayerControl.pc.transform.position, _createdSupportObjs.Count).ToList();
        for(int i = 0; i < _createdSupportObjs.Count; i++)
        {
            Utility.GenerateSupportObj(_createdSupportObjs[i], createdObjsPositions[i], Quaternion.identity);
        }
    }
}
