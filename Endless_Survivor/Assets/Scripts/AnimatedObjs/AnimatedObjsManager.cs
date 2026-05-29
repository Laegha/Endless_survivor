using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedObjsManager : MonoBehaviour
{
    static AnimatedObjsManager instance;
    public static AnimatedObjsManager aom { get { return instance; } }
    List<TransformFollowHandler> _animatedObjsFollowing = new List<TransformFollowHandler>();
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }
    public CustomAnimator SpawnAnimatedObj(AnimatedObjConfig config, bool isBlank = false)
    {
        if ((config.animation == null || config.animation.Frames.Length == 0) && !isBlank)
            return null;
        var instantiatedObj = Instantiate(GameManager.gm.prefabHolder.Prefabs["AnimatedObject"]);
        if (config.animatedObjParentTransform != null)
            _animatedObjsFollowing.Add(new(instantiatedObj.transform, config.animatedObjParentTransform, config.copyPosition, config.copyRotation, config.animatedObjPosition));
        if(config.animatedObjDuration >= 0)
        {
            Destroy(instantiatedObj.gameObject, config.animatedObjDuration);
        }
        Vector2 objStartPos = config.copyPosition ? config.animatedObjParentTransform.position + config.animatedObjPosition : config.animatedObjPosition;
        instantiatedObj.transform.position = objStartPos;
        instantiatedObj.transform.rotation = config.animatedObjRotation;
        var objAnimator = instantiatedObj.GetComponent<CustomAnimator>();
        if(config.animation != null && config.animation.Frames.Length > 0 && isBlank)
        {
            objAnimator.AddAnimations(new() { config.animation });
            objAnimator.ChangeAnim(config.animation.AnimationName);

        }
        return objAnimator;
    }
    private void Update()
    {
        var animatedObjsFollowingCopy = new List<TransformFollowHandler>(_animatedObjsFollowing);
        foreach(var animatedObjFollowing in animatedObjsFollowingCopy)
        {
            if(animatedObjFollowing.parent == null && animatedObjFollowing.child != null)
                DestroyImmediate(animatedObjFollowing.child.gameObject);
            if(animatedObjFollowing.child == null)
            {
                _animatedObjsFollowing.Remove(animatedObjFollowing);
                continue;
            }
            animatedObjFollowing.Update();
        }
    }
}
