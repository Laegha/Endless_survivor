using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MapGenerationConfig", menuName = "ScriptableObjects/Map/Generation config", order = 0)]
public class MapGenerationConfig : ScriptableObject
{
    [SerializeField] RandomBetweenTwoConstants _biomeSize;
    [SerializeField] List<BiomeData> _possibleBiomes;
    [SerializeField] int _wavesBetweenGenerations;

    public RandomBetweenTwoConstants BiomeSize { get { return _biomeSize; } }
    public List<BiomeData> PossibleBiomes { get { return _possibleBiomes; }}
    public int WavesBetweenGenerations { get { return _wavesBetweenGenerations; }}
}
