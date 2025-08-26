using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] CharacterData characterData;
    [SerializeField] CustomAnimator animator;

    private void Start()
    {
        animator.AddAnimations(new List<CustomAnimation>{characterData.Animations.Find(x => x.AnimationName == "Idle")});
        animator.ChangeAnim("Idle");
    }

}
