using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Row", menuName = "ScriptableObjects/RowData", order = 20)]
public class ObjectRowData : ScriptableObject
{
    [SerializeField] float _distBetweenElements;
    [SerializeField] float _rowAngle;
    [Tooltip("If the row is following something, this is the ammount of seconds before they start following")][SerializeField] float _rowFollowDelay;
    [Tooltip("If the row is following something, this is the offset of the first object of the row from the object")][SerializeField] Vector2 _rowOffsetFromPlayer;
    public float RowFollowDelay { get { return _rowFollowDelay; } }
    public Vector2 RowOffsetFromPlayer {  get { return _rowOffsetFromPlayer; } }
    //maybe add a rate at which row elements have they're position updated / frames needed to regiter an idle position (prevPos == newPos)
    Vector2 _rowDirection => Utility.GetPointInCircle(1, _rowAngle).normalized;

    public Vector2 GetObjRowPos(int element)
    {
        return _rowDirection * _distBetweenElements * element;
    }
}
