using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunExit : MonoBehaviour
{
    [SerializeField] CustomAnimator _exitAnimator;
    [SerializeField] CustomAnimation _idleAnimation;
    [SerializeField] CustomAnimation _openingAnimation;
    [SerializeField] CustomAnimation _openAnimation;

    bool _isOpen = false;

    private void Awake()
    {
        CustomAnimation openingAnimation = new(_exitAnimator, _openingAnimation);
        openingAnimation.Events.Add(new(null, openingAnimation.Frames.Length - 1, () => _isOpen = true));
        openingAnimation.Events.Add(new(null, openingAnimation.Frames.Length - 1, () => _exitAnimator.ChangeAnim(_openAnimation.AnimationName)));
        _exitAnimator.AddAnimations(new(){_idleAnimation, openingAnimation, _openAnimation });
    }
    private void Start()
    {
        _exitAnimator.ChangeAnim(_idleAnimation.AnimationName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root != PlayerControl.pc.transform || _isOpen || _exitAnimator.CurrAnim.AnimationName == _openingAnimation.AnimationName)
            return;
        _exitAnimator.ChangeAnim(_openingAnimation.AnimationName);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.root != PlayerControl.pc.transform || !_isOpen)
            return;
        
        GameProgressionManager.gpm.EndRun(PlayerControl.pc.CharacterData.ExitRunAnimation, PlayerControl.pc.CharacterData.ExitRunParticles, GameProgressionManager.RunEndType.Win);
    }
}
