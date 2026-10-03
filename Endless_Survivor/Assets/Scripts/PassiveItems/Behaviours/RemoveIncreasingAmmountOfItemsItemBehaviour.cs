using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RemoveIncreasingAmmountOfItemsItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => 1;
    [Tooltip("The formula is removedItems = increaseFactor * copiesOfThis + 1")][SerializeField] float _increaseFactor;
    [SerializeField] ParticleSystem _removedItemParticles;
    [SerializeReference] IPattern _particlesPattern;

    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var removeItemsOriginal = original as RemoveIncreasingAmmountOfItemsItemBehaviour;
        _increaseFactor = removeItemsOriginal._increaseFactor;
        _removedItemParticles = removeItemsOriginal._removedItemParticles;
        _particlesPattern = removeItemsOriginal._particlesPattern;
        behaviourManager.onPicked += RemoveItems;
    }
    void RemoveItems()
    {
        var itemsDistinctToThis = PlayerControl.pc.PassiveItemManager.RemovableItems.Where(item => item.ItemData != BehaviourManager.PassiveItem.ItemData).ToList();
        Debug.Log(itemsDistinctToThis[0].ItemData.ItemName);
        int thisCopies = PlayerControl.pc.PassiveItemManager.RemovableItems.Count - itemsDistinctToThis.Count;
        int removedItemCount = (int) (_increaseFactor * thisCopies + 1);

        List<Vector2> particlesPositions = _particlesPattern.GetPositions(PlayerControl.pc.transform.position, removedItemCount).ToList();
        for (int i = 0; i < removedItemCount; i++)
        {
            var removedItem = itemsDistinctToThis[Random.Range(0, itemsDistinctToThis.Count)];
            PlayerControl.pc.PassiveItemManager.RemovePassiveItem(removedItem);
            itemsDistinctToThis.Remove(removedItem);
            if (_removedItemParticles == null || removedItem.ItemData.ItemSprite == null)
                continue;
            ParticleConfig removedItemParticleConfig = new(_removedItemParticles, particlesPositions[i], Quaternion.identity, _removedItemParticles.main.duration, null, false, false);
            var createdParticles = ParticleManager.pm.SpawnParticles(removedItemParticleConfig);
            for (int j = 0; j < createdParticles.textureSheetAnimation.spriteCount; j++)
            {
                createdParticles.textureSheetAnimation.RemoveSprite(j);
            }
            createdParticles.textureSheetAnimation.AddSprite(removedItem.ItemData.ItemSprite);
        }
    }
    public override void RemoveBehaviour()
    {

    }
}
