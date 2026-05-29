using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCostumeManager : MonoBehaviour
{
    [SerializeField] Transform _costumeHolder;
    CharacterCostumeSettings _costumeSettings;
    Dictionary<Costume, ActiveCostumeInfo> _activeCostumes = new();
    Vector2 _rbLastDir = Vector2.zero;
    public CharacterCostumeSettings CostumeSettings { set { _costumeSettings = value; } }

    private void Update()
    {
        //get player rb direction
        Vector2 playerRbDirection = PlayerControl.pc.PlayerRb.velocity.normalized;
        Vector2 prevOrientation = Mathf.Abs(_rbLastDir.x) > Mathf.Abs(_rbLastDir.y) ? new Vector2(1, 0) * _rbLastDir.x / Mathf.Abs(_rbLastDir.x) : new Vector2(0, 1) * _rbLastDir.y / Mathf.Abs(_rbLastDir.y);
        Vector2 currOrientation = Mathf.Abs(playerRbDirection.x) > Mathf.Abs(playerRbDirection.y) ? new Vector2(1, 0) * playerRbDirection.x / Mathf.Abs(playerRbDirection.x) : new Vector2(0, 1) * playerRbDirection.y / Mathf.Abs(playerRbDirection.y);
        prevOrientation = _rbLastDir == Vector2.zero ? Vector2.zero : prevOrientation;
        currOrientation = playerRbDirection == Vector2.zero ? Vector2.zero : currOrientation;
        _rbLastDir = playerRbDirection;
        //if direction hasn't changed since last update, return
        if (prevOrientation == currOrientation)
            return;
        var activeCostumesCopy = new Dictionary<Costume, ActiveCostumeInfo> (_activeCostumes);
        foreach(var activeCostume in activeCostumesCopy)
        {
            //if there's no animation, continue
            if(currOrientation == Vector2.zero)
            {
                if (activeCostume.Key.IdleAnim.Frames.Length == 0)
                    continue;
                foreach (var animator in activeCostume.Value.costumeAnimators)
                {
                    animator.EndAnimation(animator.CurrAnim.AnimationName);
                    animator.ChangeAnim(activeCostume.Key.IdleAnim.AnimationName);
                }
                continue;
            }
            foreach(var animator in activeCostume.Value.costumeAnimators)
            {
                CustomAnimation costumeAnim = activeCostume.Key.MovingAnims.GetAnim(playerRbDirection);
                animator.EndAnimation(animator.CurrAnim.AnimationName);
                animator.ChangeAnim(costumeAnim);
            }
        }
    }

    public CustomAnimator AddCostume(Costume costume)
    {
        if (!_activeCostumes.ContainsKey(costume))
            _activeCostumes.Add(costume, new ActiveCostumeInfo());
        ActiveCostumeInfo costumeActiveInfo = _activeCostumes[costume];
        _activeCostumes[costume].costumeStacks++;
        if (_activeCostumes[costume].costumeStacks > costume.MaxStacks)
            return _activeCostumes[costume].costumeAnimators[_activeCostumes[costume].costumeStacks - costume.MaxStacks];
        
        AnimatedObjConfig costumeConfig = new(null, PlayerControl.pc.transform.position, Quaternion.identity, -1, null, false, false);
        CustomAnimator costumeAN = AnimatedObjsManager.aom.SpawnAnimatedObj(costumeConfig, true);
        costumeAN.transform.root.SetParent(_costumeHolder);

        Vector2 costumePosition = GetCostumePosition(costume, costumeActiveInfo.costumeStacks - 1);
        costumeAN.transform.localPosition = costumePosition;
        costumeAN.AddAnimations(costume.NonNullAnimations);
        costumeAN.ChangeAnim(costume.IdleAnim.AnimationName);
        _activeCostumes[costume].costumeAnimators.Add(costumeAN);
        return costumeAN;
    }

    public void RemoveCostume(Costume removedCostume)
    {
        _activeCostumes[removedCostume].costumeStacks--;
        int extraAnimators = _activeCostumes[removedCostume].costumeAnimators.Count - _activeCostumes[removedCostume].costumeStacks;
        if(extraAnimators <= 0)
            return;
        for(int i = 0; i < extraAnimators; i++)
        {
            int destroyedId = _activeCostumes[removedCostume].costumeAnimators.Count -1;
            Destroy(_activeCostumes[removedCostume].costumeAnimators[destroyedId]);
            _activeCostumes[removedCostume].costumeAnimators.RemoveAt(destroyedId);//by removing the last time we ensure the next iteration of this for removes th next one
        }

        if (_activeCostumes[removedCostume].costumeStacks == 0)
        {
            _activeCostumes.Remove(removedCostume);

        }
    }

    Vector2 GetCostumePosition(Costume costume, int offsetIndex)
    {
        var costumeActiveInfo = _activeCostumes[costume];
        return _costumeSettings.CostumeOffsets[costume.CostumePosition] + costume.OffsetFromPositionByStacks[offsetIndex];
    }


}

class ActiveCostumeInfo
{
    public List<CustomAnimator> costumeAnimators = new();
    public int costumeStacks;
}