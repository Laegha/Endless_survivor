using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : ScriptableObject
{
    [SerializeField] Sprite _menuImage;
    [SerializeField] GameObject _characterPrefab;
    [SerializeField] GameObject[] _initialGuns;
    [SerializeField] GameObject[] _initialPassives;
    
    public Sprite MenuImage {  get { return _menuImage; } }
    public GameObject CharacterPrefab {  get { return _characterPrefab; } }
    public GameObject[] InitialGuns { get { return _initialGuns; } }
    public GameObject[] InitialPassives { get { return _initialPassives; }
}