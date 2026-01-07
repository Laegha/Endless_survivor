using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BiomeData", menuName = "ScriptableObjects/Map/Biome Data", order = 1)]
public class BiomeData : ScriptableObject
{
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

    //for no decoration chance, add element with no sprite
    [SerializeField] List<RouletteElement<Sprite>> _floorDecorationTiles;
    [SerializeField] List<RouletteElement<Sprite>> _topWallDecorationTiles;
    [SerializeField] List<RouletteElement<Sprite>> _rightWallDecorationTiles;
    [SerializeField] List<RouletteElement<Sprite>> _bottomWallDecorationTiles;
    [SerializeField] List<RouletteElement<Sprite>> _leftWallDecorationTiles;

    [SerializeField] List<RouletteElementChance<TileSupportObj>> _tileGeneratableSupportObjs;

    [Tooltip("num is the wave number since which each wave can proc")]
    [SerializeField] List<GenericNumHolder<Wave>> _biomeEnemyWaves;
    [SerializeField] List<EnemyData> _biomeChampions;
    [SerializeField] List<EnemyData> _biomeBosses;

    [SerializeField] Sprite _championIndicator;
    [SerializeField] Sprite _bossIndicator;

    [SerializeField] List<RouletteElement<TileSupportObj>> _possibleSupportObjectsPerTile;

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
}
