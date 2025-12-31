using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SupportObject", menuName = "ScriptableObjects/Support Object", order = 2)]
public class SupportObjectData : ScriptableObject
{
    [SerializeReference] List<SupportObjectBehaviour> _supportObjBehaviours = new();
    [SerializeField] CustomAnimation _idleAnimation;
    [SerializeField] ColliderData[] _supportObjColliders;

    public List<SupportObjectBehaviour> SupportObjBehaviours { get { return _supportObjBehaviours; } }
    public CustomAnimation IdleAnimation { get { return _idleAnimation; } }
    public void TransferData(SupportObjectControl supportObjControl)
    {
        if(_supportObjColliders.Length > 0)
        {
            CreateColldiers(supportObjControl);
        }

        supportObjControl.Animator.AddAnimations(new List<CustomAnimation>{_idleAnimation});
        supportObjControl.Animator.ChangeAnim(_idleAnimation);
        supportObjControl.BehaviourManager.SetBehaviours(_supportObjBehaviours, this);

    }

    void CreateColldiers(SupportObjectControl supportObjControl)
    {
        for (int i = 0; i < _supportObjColliders.Length; i++)
        {
            var colliderObj = new GameObject("Collider " + i);
            colliderObj.transform.SetParent(supportObjControl.ColliderHolder);
            colliderObj.AddComponent<SupportObjectCollisionDetector>().BehaviourManager = supportObjControl.BehaviourManager;

            var colliderData = _supportObjColliders[i];
            if (colliderData.colType == ColliderData.ColliderType.Capusle)
            {
                var colliderComponent = colliderObj.AddComponent<CapsuleCollider2D>();
                colliderComponent.direction = colliderData.capsuleDirection;
                colliderComponent.size = colliderData.size;
                colliderComponent.offset = colliderData.position;
            }
            else
            {
                var colliderComponent = colliderObj.AddComponent<BoxCollider2D>();
                colliderComponent.size = colliderData.size;
                colliderComponent.offset = colliderData.position;
            }
        }
    }
}
