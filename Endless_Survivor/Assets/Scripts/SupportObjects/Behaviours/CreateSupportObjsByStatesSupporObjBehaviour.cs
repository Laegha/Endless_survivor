using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CreateSupportObjsByStatesSupporObjBehaviour : UseAreaAroundSupportObjBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] List<CreateSupportObjState> _states = new List<CreateSupportObjState>();
    [SerializeField] Vector2 _stateIndicatorOffset;
    [SerializeField] float _stateDuration;
    [SerializeField] int _resetStateIndex;
    float _stateTimer;
    int _currStateIndex = 0;
    List<GameObject> _currStateObjs = new();
    GameObject _currStateIndicator;
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var createSupportObjsStatesOriginal = original as CreateSupportObjsByStatesSupporObjBehaviour;
        foreach (var state in createSupportObjsStatesOriginal._states)
        {
            CreateSupportObjState newState = new(ObjControl.Animator, state);
            _states.Add(newState);
            ObjControl.Animator.AddAnimations(new() { newState.StateIdleAnim, newState.StateChangeAnim });

        }
        _stateIndicatorOffset = createSupportObjsStatesOriginal._stateIndicatorOffset;
        _stateDuration = createSupportObjsStatesOriginal._stateDuration;
        _stateTimer = _stateDuration;
        _resetStateIndex = createSupportObjsStatesOriginal._resetStateIndex;
        GoToResetState();
        OnUpdate += DecreaseStateTimer;
        OnObjEnterArea += CheckPlayerInteract;
    }
    void CheckPlayerInteract(GameObject interactingObj)
    {
        if (interactingObj.transform.root != PlayerControl.pc.transform)
            return;
        GoToResetState();
    }
    void DecreaseStateTimer()
    {
        if (_stateTimer < 0)
            return;

        _stateTimer -= Time.deltaTime;
        if (_stateTimer > 0)
            return;

        GoToNextState();
    }
    void GoToNextState()
    {
        if (_currStateIndex == _states.Count - 1)
            return;
        _currStateIndex++;
        _stateTimer = _stateDuration;
        var newState = _states[_currStateIndex];
        StartNewState(newState);
    }
    void GoToResetState()
    {
        var prevState = _states[_currStateIndex];

        float animDuration = prevState.StateChangeAnim.AnimDuration;
        _stateTimer = _stateDuration + animDuration;

        if (_states.Any(x => x.StateIdleAnim.AnimationName == ObjControl.Animator.CurrAnim.AnimationName))
            ObjControl.Animator.EndAnimation(ObjControl.Animator.CurrAnim.AnimationName);

        ObjControl.Animator.ChangeAnim(prevState.StateChangeAnim);
        var resetState = _states[_resetStateIndex];
        GameManager.gm.DelayAction(animDuration, () => StartNewState(resetState), () => ObjControl == null);
        _currStateIndex = _resetStateIndex;
    }

    void StartNewState(CreateSupportObjState state)
    {
        if (_states.Any(x => x.StateChangeAnim.AnimationName == ObjControl.Animator.CurrAnim.AnimationName))
            ObjControl.Animator.EndAnimation(ObjControl.Animator.CurrAnim.AnimationName);
        ObjControl.Animator.ChangeAnim(state.StateIdleAnim);
        foreach(var oldObj in _currStateObjs)
            ObjectDestroyingManager.odm.DestroyObj(oldObj);
        
        ObjectDestroyingManager.odm.DestroyObj(_currStateIndicator);
        _currStateIndicator = AnimatedObjsManager.aom.SpawnAnimatedObj(new(state.StateIndicatorAnim, _stateIndicatorOffset, Quaternion.identity, -1, ObjControl.transform.transform, true, true, 50)).gameObject;
        if (state.StateCreatedObjs.Count == 0)
            return;
        List<Vector2> newObjsPositions = state.CreatedObjsPattern.GetPositions(ObjControl.transform.position, state.StateCreatedObjs.Count).ToList();
        for(int i = 0; i < state.StateCreatedObjs.Count; i++)
        {
            var newObjData = state.StateCreatedObjs[i];
            var generatedObj = Utility.GenerateSupportObj(newObjData, newObjsPositions[i], Quaternion.identity);
            _currStateObjs.Add(generatedObj.gameObject);
        }
    }
}

[Serializable]
public class CreateSupportObjState
{
    [SerializeField] CustomAnimation _stateIdleAnim;
    [SerializeField] CustomAnimation _stateChangeAnim;
    [SerializeField] CustomAnimation _stateIndicatorAnim;
    [SerializeField] List<SupportObjectData> _stateCreatedObjs;
    [SerializeReference] IPattern _createdObjsPattern;

    public CustomAnimation StateIdleAnim { get { return _stateIdleAnim; } }
    public CustomAnimation StateChangeAnim { get { return _stateChangeAnim; } }
    public CustomAnimation StateIndicatorAnim { get { return _stateIndicatorAnim; } }
    public List<SupportObjectData> StateCreatedObjs {  get { return _stateCreatedObjs; } }
    public IPattern CreatedObjsPattern {  get { return _createdObjsPattern; } }
    public CreateSupportObjState(CustomAnimator animator, CustomAnimation stateIdleAnim, CustomAnimation stateChangeAnim, CustomAnimation stateIndicatorAnim, List<SupportObjectData> stateCreatedObjs, IPattern createdObjsPattern)
    {
        _stateIdleAnim = new(animator, stateIdleAnim);
        _stateChangeAnim = new(animator, stateChangeAnim);
        _stateIndicatorAnim = new(animator, stateIndicatorAnim);
        _stateCreatedObjs = stateCreatedObjs;
        _createdObjsPattern = createdObjsPattern;
    }
    public CreateSupportObjState(CustomAnimator animator, CreateSupportObjState original)
    {
        _stateIdleAnim = new(animator, original._stateIdleAnim);
        _stateChangeAnim = new(animator, original._stateChangeAnim);
        _stateIndicatorAnim = new(animator, original._stateIndicatorAnim);
        _stateCreatedObjs = new(original._stateCreatedObjs);
        _createdObjsPattern = original._createdObjsPattern;
    }
}