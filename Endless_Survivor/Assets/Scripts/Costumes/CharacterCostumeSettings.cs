using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CostumeSettings", menuName = "ScriptableObjects/Costumes/CostumeSettings", order = 0)]
public class CharacterCostumeSettings : ScriptableObject
{
    public enum CostumePosition
    {
        head,
        chest,
        waist,
        rightShoulder,
        leftShoulder
    }
    [SerializeField] Vector2 _headOffset;
    [SerializeField] Vector2 _chestOffset;
    [SerializeField] Vector2 _waistOffset;
    [SerializeField] Vector2 _rightShoulderOffset;
    [SerializeField] Vector2 _leftShoulderOffset;

    public Dictionary<CostumePosition, Vector2> CostumeOffsets
    {
        get
        {
            Dictionary<CostumePosition,Vector2> result = new()
            {
                {CostumePosition.head, _headOffset},
                {CostumePosition.chest, _chestOffset},
                {CostumePosition.waist, _waistOffset},
                {CostumePosition.rightShoulder, _rightShoulderOffset},
                {CostumePosition.leftShoulder, _leftShoulderOffset}
            };
            return result;
        }
    }
}
