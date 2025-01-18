using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "ScriptableObjects/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    [SerializeField] Sprite _menuImage;
    [SerializeField] GameObject _characterPrefab;
    [SerializeField] GameObject[] _characterHands;
    [SerializeField] WeaponData[] _initialWeapons;
    [SerializeField] GameObject[] _initialPassives;

    [SerializeField] PlayerStats _playerStats;
    
    public Sprite MenuImage {  get { return _menuImage; } }
    public GameObject CharacterPrefab {  get { return _characterPrefab; } }
    public GameObject[] CharacterHands { get { return _characterHands; } }
    public WeaponData[] InitialWeapons { get { return _initialWeapons; } }
    public GameObject[] InitialPassives { get { return _initialPassives; } }
    public PlayerStats PlayerStats{ get { return _playerStats; } }
}