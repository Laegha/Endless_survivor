using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CostumePosition = CharacterCostumeSettings.CostumePosition;

[CreateAssetMenu(fileName = "New Costume", menuName = "ScriptableObjects/Costumes/Costume", order = 1)]
public class Costume : ScriptableObject
{
    [SerializeField] CostumePosition _costumePosition;
    [SerializeField] List<Vector2> _offsetFromPositionByStacks;
    [SerializeField] CustomAnimation _idleAnimation;
    [SerializeField] DirectionalCustomAnimation _movingAnimations;
    [SerializeField] bool _syncronizeWithPlayerAnim;
    [SerializeField] int _renderOffset = 0;


    public CostumePosition CostumePosition { get { return _costumePosition; } }
    public List<Vector2> OffsetFromPositionByStacks { get { return _offsetFromPositionByStacks; } }
    public int MaxStacks => _offsetFromPositionByStacks.Count;
    public CustomAnimation IdleAnim { get { return _idleAnimation; } }
    public DirectionalCustomAnimation MovingAnims { get { return _movingAnimations; } }
    public List<CustomAnimation> NonNullAnimations 
    { 
        get 
        {
            DirectionalCustomAnimation movingAnimsInstance = new(null, _movingAnimations);
            var result = new List<CustomAnimation>(movingAnimsInstance.NonNullAnimations);
            if (_idleAnimation.Frames.Length > 0)
                result.Add(_idleAnimation);
            return result; 
    
        } 
    }
    public bool SynchronizeWithPlayerAnim { get { return _syncronizeWithPlayerAnim;} }
    public int RenderOffset { get { return _renderOffset; } }
}
