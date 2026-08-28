using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayerInRowSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 1;
    static Dictionary<ObjectRowData, ObjectRow> _followerRows = new();
    [SerializeField] ObjectRowData _rowData;
    [SerializeField] CustomAnimation _idleAnimation;
    [SerializeField] DirectionalCustomAnimation _movingAnimations; 
    ObjectRowElement _myRowElement;
    List<Vector2> _playerPositionsBuffer = new();
    float _delayTimer;

    List<CustomAnimation> _animations => new(_movingAnimations.NonNullAnimations) { _idleAnimation };
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var followOriginal = original as FollowPlayerInRowSupportObjBehaviour;
        _rowData = followOriginal._rowData;
        _idleAnimation = new(ObjControl.Animator, followOriginal._idleAnimation);
        _movingAnimations = new(ObjControl.Animator,followOriginal._movingAnimations);
        ObjControl.Animator.AddAnimations(_animations);
        _delayTimer = Mathf.Clamp(_rowData.RowFollowDelay, 0, Mathf.Infinity);

        OnStart += AddToRow;
        OnFixedUpdate += UpdatePlayerPosBuffer;
        OnFixedUpdate += UpdateRow;
        OnUpdate += PlayAnim;
        OnDestroyed += RemoveFromRow;
    }
    void AddToRow()
    {
        if (!_followerRows.ContainsKey(_rowData))
            _followerRows.Add(_rowData, new(_rowData, GetRowStartPos));
        _myRowElement = _followerRows[_rowData].AddObj(ObjControl.gameObject);
    }   
    void UpdatePlayerPosBuffer()
    {
        //If i'm not the head of the row, don't update it so it doesn't get updated multiple times per frame
        if (_followerRows[_rowData].RowObjs[0] != _myRowElement)
            return;
        _playerPositionsBuffer.Add(PlayerControl.pc.transform.position);
        if (_delayTimer >= 0)
        {
            _delayTimer -= Time.deltaTime;
            return;
        }
        _playerPositionsBuffer.RemoveAt(0);

    }
    void UpdateRow()
    {
        //If i'm not the head of the row, don't update it so it doesn't get updated multiple times per frame
        if (_followerRows[_rowData].RowObjs[0] != _myRowElement)
            return;
        //Add delay?
        _followerRows[_rowData].UpdateObjsPositions();
    }
    void PlayAnim()
    {
        Vector2 movingDir = _myRowElement.MovingDirection;
        CustomAnimation newAnim;
        if (movingDir != Vector2.zero)
            newAnim = _movingAnimations.GetAnim(movingDir);
        else
            newAnim = _idleAnimation?.Frames.Length > 0 ? _idleAnimation : _movingAnimations.GetAnim(Vector2.down);

        var currAnimName = ObjControl.Animator.CurrAnim.AnimationName;
        if (newAnim.AnimationName == currAnimName)
            return;

        if (_animations.Any(anim => anim.AnimationName == currAnimName))
        {
            ObjControl.Animator.EndAnimation(currAnimName);
        }
        ObjControl.Animator.ChangeAnim(newAnim.AnimationName);
    }
    Vector2 GetRowStartPos()
    {
        //add delay?
        List<Vector2> playerPosBufferCopy = new(_playerPositionsBuffer);
        Vector2 playerVelocity = PlayerControl.pc.PlayerRb.velocity;
        //make offset adjust with angle (if player.velocity is (0, 1), invert offset, if it is (1, 0) rotate it 90º) 
        float playerAngle = playerVelocity == Vector2.zero ? 0 : Utility.GetAngleFromPointInCircle(playerVelocity.normalized, false) - 270;
        Vector2 rotatedStartPos = Utility.RotatePoint(_rowData.RowOffsetFromPlayer, playerAngle);
        Vector2 startPos = playerPosBufferCopy[0] + rotatedStartPos;
        return startPos;
    }
    void RemoveFromRow()
    {
        _followerRows[_rowData].RowObjs.Remove(_myRowElement);
        if(_followerRows[_rowData].RowObjs.Count == 0)
            _followerRows.Remove(_rowData);
    }
}
