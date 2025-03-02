using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "ScriptableObjects/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    [SerializeField] Sprite _menuImage;
    [SerializeField] GameObject _characterPrefab;
    [SerializeField] GameObject[] _characterHands;
    [InspectorLabel("Initial weapons and passive items")]
    [SerializeField] WeaponData[] _initialWeapons;
    [SerializeField] GameObject[] _initialPassives;

    [InspectorLabel("Character stats")]
    [SerializeField] PlayerStats _playerStats;

    [InspectorLabel("Animations")]
    [SerializeField] CoordinateAnimation _idle;
    [SerializeField] CoordinateAnimation _frontMoving;
    [SerializeField] CoordinateAnimation _rightMoving;
    [SerializeField] CoordinateAnimation _backMoving;
    [SerializeField] CoordinateAnimation _leftMoving;

    public Sprite MenuImage {  get { return _menuImage; } }
    public GameObject CharacterPrefab {  get { return _characterPrefab; } }
    public GameObject[] CharacterHands { get { return _characterHands; } }
    public WeaponData[] InitialWeapons { get { return _initialWeapons; } }
    public GameObject[] InitialPassives { get { return _initialPassives; } }
    public PlayerStats PlayerStats{ get { return _playerStats; } }
    public List<CoordinateAnimation> Animations { get {
            return new List<CoordinateAnimation>
            {
                _idle,
                _frontMoving,
                _rightMoving,
                _backMoving,
                _leftMoving,
            };
    } } 
}