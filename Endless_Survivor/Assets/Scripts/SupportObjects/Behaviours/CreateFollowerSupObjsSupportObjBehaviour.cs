using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFollowerSupObjsSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] List<FollowSupObjSupportObjBehaviourInfo> _followersInfo;

    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var createFollowersOriginal = original as CreateFollowerSupObjsSupportObjBehaviour;
        _followersInfo = new(createFollowersOriginal._followersInfo);
        OnStart += CreateFollowers;
    }

    void CreateFollowers()
    {
        foreach(var followerInfo in  _followersInfo)
        {
            SupportObjectControl followerControl = Utility.GenerateSupportObj(followerInfo.FollowerSupportObjData, (Vector2)ObjControl.transform.position + followerInfo.FollowerStartOffset, Quaternion.identity);
            FollowTrSupportObjBehaviour newBehaviour = new();
            newBehaviour.Target = ObjControl.transform;
            newBehaviour.MoveSpeed = () => followerInfo.FollowerSpeed;
            newBehaviour.Initiate(followerControl, followerInfo.FollowingBehaviour);
            followerControl.BehaviourManager.Behaviours.Add(newBehaviour);
            newBehaviour.OnStart?.Invoke();

        }
    }
}
[Serializable]
class FollowSupObjSupportObjBehaviourInfo
{
    [SerializeField] SupportObjectData _followerSupportObjData;
    [SerializeField] Vector2 _followerStartOffset;
    [SerializeField] float _followerSpeed;
    [SerializeField] FollowTrSupportObjBehaviour _followingBehaviour;

    public SupportObjectData FollowerSupportObjData {  get { return _followerSupportObjData; } }
    public Vector2 FollowerStartOffset { get { return _followerStartOffset; } }
    public float FollowerSpeed { get { return _followerSpeed; } }
    public FollowTrSupportObjBehaviour FollowingBehaviour { get { return _followingBehaviour; } }
}