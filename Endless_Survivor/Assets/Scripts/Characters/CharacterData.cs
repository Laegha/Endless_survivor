using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "ScriptableObjects/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    [SerializeField] Sprite _menuImage;
    [SerializeField] string _characterName;
    [SerializeField] Sprite[] _characterHands;

    [InspectorLabel("Initial weapons and passive items")]
    [SerializeField] WeaponData[] _initialWeapons;
    [SerializeField] int _initialMaxWeapons = 2;
    [SerializeField] GameObject[] _initialPassives;

    [InspectorLabel("Character stats")]
    [SerializeField] PlayerStats _playerStats;

    [InspectorLabel("Collider")]
    [SerializeField] Vector2 _colliderSize = Vector2.one * .5f;
    [SerializeField] Vector2 _colliderOffset = Vector2.down * 0.41f;
    [SerializeField] CapsuleDirection2D _colliderDirection = CapsuleDirection2D.Vertical;

    [InspectorLabel("Animations")]
    [SerializeField] CustomAnimation _idle;
    [SerializeField] CustomAnimation _frontMoving;
    [SerializeField] CustomAnimation _rightMoving;
    [SerializeField] CustomAnimation _backMoving;
    [SerializeField] CustomAnimation _leftMoving;

    [InspectorLabel("Sounds")]
    [SerializeField] SFXInfo _onHitSound;
    [SerializeField] SFXInfo _onDeathSound;

    public Sprite MenuImage {  get { return _menuImage; } }
    public string CharacterName { get { return _characterName; } }
    public Sprite[] CharacterHands { get { return _characterHands; } }
    public WeaponData[] InitialWeapons { get { return _initialWeapons; } }
    public int InitialMaxWeapons { get { return _initialMaxWeapons;} }
    public GameObject[] InitialPassives { get { return _initialPassives; } }
    public PlayerStats PlayerStats{ get { return _playerStats; } }
    public Vector2 ColliderSize {  get { return _colliderSize; } }
    public Vector2 ColliderOffset {  get { return _colliderOffset; } }
    public CapsuleDirection2D ColliderDirection {  get { return _colliderDirection; } }
    public List<CustomAnimation> Animations { get {
            return new List<CustomAnimation>
            {
                _idle,
                _frontMoving,
                _rightMoving,
                _backMoving,
                _leftMoving,
            };
    }
    }
    public SFXInfo OnHitSound { get { return _onHitSound; } }
    public SFXInfo OnDeathSound { get { return _onDeathSound; } }
}