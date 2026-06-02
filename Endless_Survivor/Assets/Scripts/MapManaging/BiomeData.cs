using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BiomeData", menuName = "ScriptableObjects/Map/Biome Data", order = 1)]
public class BiomeData : ScriptableObject
{
    [SerializeField] BiomeBackgroundData _backgroundData;
    [SerializeField] Sprite _floorTile;
    [Header("Walls")]
    [SerializeField] Sprite _topWallTile;
    [SerializeField] Sprite _rightWallTile;
    [SerializeField] Sprite _bottomWallTile;
    [SerializeField] Sprite _leftWallTile;
    [Header("Wall Corners")]
    [SerializeField] Sprite _cornerTopRightClosed;
    [SerializeField] Sprite _cornerTopRightOpen;
    [SerializeField] Sprite _cornerRightBotClosed;
    [SerializeField] Sprite _cornerRightBotOpen;
    [SerializeField] Sprite _cornerBotLeftClosed;
    [SerializeField] Sprite _cornerBotLeftOpen;
    [SerializeField] Sprite _cornerLeftTopClosed;
    [SerializeField] Sprite _cornerLeftTopOpen;

    [SerializeField] float _decorationOnTileChance;
    [SerializeField] List<RouletteElementChance<TileDecorationData>> _floorDecorationDatas;

    [SerializeField] float _supportObjOnTileChance;
    [SerializeField] List<RouletteElementChance<TileSupportObjData>> _possibleSupportObjectsPerTile;

    [Tooltip("num is the wave number since which each wave can proc")]
    [SerializeField] List<RouletteElementChance<EnemySpawnInfo>> _biomeEnemies;
    [SerializeField] CustomAnimation _bossInvokerAnimation;
    [SerializeField] bool _usesBoxCollider;
    [SerializeField] Vector2 _boxColliderSize;
    [SerializeField] bool _usesCircleCollider;
    [SerializeField] float _circleColliderRadius;
    [SerializeField] Vector2 _colliderOffset;
    [SerializeField] List<EnemyInvokationInfo> _biomeChampions;
    [SerializeField] List<EnemyInvokationInfo> _biomeBosses;


    public BiomeBackgroundData BackgroundData{ get { return _backgroundData; } }
    public Sprite FloorTile { get { return _floorTile; } }
    public Sprite TopWallTile { get { return _topWallTile; } }
    public Sprite RightWallTile { get { return _rightWallTile; } }
    public Sprite BottomWallTile { get {  return _bottomWallTile; } }
    public Sprite LeftWallTile { get { return _leftWallTile; } }

    public Sprite CornerTopRightClosed { get { return _cornerTopRightClosed; } }
    public Sprite CornerTopRightOpen { get { return _cornerTopRightOpen; } }
    public Sprite CornerRightBotClosed { get { return _cornerRightBotClosed; } }
    public Sprite CornerRightBotOpen { get { return _cornerRightBotOpen; } }
    public Sprite CornerBotLeftClosed { get { return _cornerBotLeftClosed; } }
    public Sprite CornerBotLeftOpen { get { return _cornerBotLeftOpen; } }
    public Sprite CornerLeftTopClosed { get { return _cornerLeftTopClosed; } }
    public Sprite CornerLeftTopOpen { get { return _cornerLeftTopOpen; } }

    public float DecorationChance { get { return _decorationOnTileChance; } }
    public List<RouletteElementChance<MapElementInfo<CustomAnimation>>> FloorDecorations { 
        get
        {
            List<RouletteElementChance<MapElementInfo<CustomAnimation>>> result = new();
            foreach (var dataChance in _floorDecorationDatas)
            {
                MapElementInfo<CustomAnimation> decorMapInfo = new(dataChance.RouletteElement.DecorationAnimation, dataChance.RouletteElement.DecorSize);
                RouletteElementChance<MapElementInfo<CustomAnimation>> rouletteElement = new(decorMapInfo, dataChance.Chance);
                result.Add(rouletteElement);
            }
            return result;
        } 
    }
    public float SupportObjChance { get { return _supportObjOnTileChance; } }
    public List<RouletteElementChance<MapElementInfo<SupportObjectData>>> SupportObjs
    {
        get
        {
            List<RouletteElementChance<MapElementInfo<SupportObjectData>>> result = new();
            foreach (var dataChance in _possibleSupportObjectsPerTile)
            {
                MapElementInfo<SupportObjectData> objMapInfo = new(dataChance.RouletteElement.SupportObj, dataChance.RouletteElement.ObjSize);
                RouletteElementChance<MapElementInfo<SupportObjectData>> rouletteElement = new(objMapInfo, dataChance.Chance);
                result.Add(rouletteElement);
            }
            return result;
        }
    }

    public List<RouletteElementChance<EnemySpawnInfo>> BiomeEnemies { get { return _biomeEnemies; } }
    public CustomAnimation BossInvokerAnimation {  get { return _bossInvokerAnimation; } }
    public bool UsesBoxCollider { get { return _usesBoxCollider; } }
    public Vector2 BoxColliderSize { get { return _boxColliderSize; } }
    public bool UsesCircleCollider { get { return _usesCircleCollider; } }
    public float CircleColldierRadius { get { return _circleColliderRadius; } }
    public Vector2 ColliderOffset { get { return _colliderOffset; } }
    public List<EnemyInvokationInfo> BiomeChampions { get {  return _biomeChampions; } }
    public List<EnemyInvokationInfo> BiomeBosses {  get { return _biomeBosses; } }
}
