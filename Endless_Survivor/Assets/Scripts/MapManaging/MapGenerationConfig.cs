using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MapGenerationConfig", menuName = "ScriptableObjects/Map/Generation config", order = 0)]
public class MapGenerationConfig : ScriptableObject
{
    [SerializeField] int _maxBiomes = 5;
    [SerializeField] RandomBetweenTwoConstants _biomeSize;
    [SerializeField] List<BiomeData> _possibleBiomes;

    [SerializeField] EnemyInvoker _bossInvokerPrefab;
    [SerializeField] MapElementSize _bossInvokerSize;

    [SerializeField] Material _regularTileMaterial;
    [SerializeField] Material _2BlendUp;
    [SerializeField] Material _2BlendRight;
    [SerializeField] Material _2BlendDown;
    [SerializeField] Material _2BlendLeft;
    [SerializeField] Material _3BlendUpLeft;
    [SerializeField] Material _3BlendUpRight;
    [SerializeField] Material _3BlendRightUp;
    [SerializeField] Material _3BlendRightDown;
    [SerializeField] Material _3BlendDownRight;
    [SerializeField] Material _3BlendDownLeft;
    [SerializeField] Material _3BlendLeftDown;
    [SerializeField] Material _3BlendLeftUp;
    [SerializeField] Material _4BlendUpRight;
    [SerializeField] Material _4BlendUpLeft;
    [SerializeField] Material _4BlendDownRight;
    [SerializeField] Material _4BlendDownLeft;

    public int MaxBiomes { get { return _maxBiomes; } }
    public RandomBetweenTwoConstants BiomeSize { get { return _biomeSize; } }
    public List<BiomeData> PossibleBiomes { get { return _possibleBiomes; }}
    public EnemyInvoker BossInvokerPrefab { get { return _bossInvokerPrefab; } }
    public MapElementSize BossInvokerSize { get { return _bossInvokerSize; } }

    public Material GetRegularMaterial()
    {
        return _regularTileMaterial;
    }

    public Material Get2BlendMatrerial(Vector2 direction)
    {
        Material result = null;
        if(direction == Vector2.up)
            result = _2BlendUp;
        if(direction == Vector2.right)
            result = _2BlendRight;
        if(direction == Vector2.down)
            result = _2BlendDown;
        if(direction == Vector2.left)
            result = _2BlendLeft;
        return result;
    }
    public Material Get3BlendMatrerial(Vector2 mainDirection, Vector2 secondaryDirection)
    {
        Material result = null;
        if(mainDirection == Vector2.up)
        {
            if(secondaryDirection == Vector2.right)
                result = _3BlendUpRight;
            if(secondaryDirection == Vector2.left)
                result = _3BlendUpLeft;
        }
        if(mainDirection == Vector2.right)
        {
            if(secondaryDirection == Vector2.up)
                result = _3BlendRightUp;
            if(secondaryDirection == Vector2.down)
                result = _3BlendRightDown;
        }
        if(mainDirection == Vector2.down)
        {
            if (secondaryDirection == Vector2.right)
                result = _3BlendDownRight;
            if (secondaryDirection == Vector2.left)
                result = _3BlendDownLeft;
            
        }
        if(mainDirection == Vector2.left)
        {
            if (secondaryDirection == Vector2.up)
                result = _3BlendLeftUp;
            if (secondaryDirection == Vector2.down)
                result = _3BlendLeftDown;
        }
        return result;
    }
    public Material Get4BlendMaterial(Vector2 direction)
    {
        Material result = null;
        if (direction == new Vector2(1, 1))
            result = _4BlendUpRight;
        if (direction == new Vector2(1, -1))
            result = _4BlendDownRight;
        if (direction == new Vector2(-1, -1))
            result = _4BlendDownLeft;
        if (direction == new Vector2(-1, 1))
            result = _4BlendUpLeft;
        return result;
    }
}
