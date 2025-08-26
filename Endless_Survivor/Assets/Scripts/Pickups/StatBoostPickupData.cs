//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[CreateAssetMenu(fileName = "New StatBoost Pickup", menuName = "ScriptableObjects/Pickups/StatBoost", order = 1)]
//public class StatBoostPickupData : PickupData
//{
//    [SerializeField] CustomAnimation _pickupAnimation;
//    [SerializeField] PlayerStats _statsBoost;
//    public override void TransferData(PickupControl pickupControl)
//    {
//        base.TransferData(pickupControl);
//        CustomAnimator pickupAnimator = pickupControl.GetComponent<PickupControl>().Animator;
//        pickupAnimator.AddAnimations(new List<CustomAnimation> { _pickupAnimation });
//        pickupAnimator.ChangeAnim(_pickupAnimation.AnimationName);

//    }
//    public override void PickUp(PickupControl pickupControl)
//    {
//        base.PickUp(pickupControl);

//        PlayerStats playerStats = PlayerControl.pc.PlayerStats;
//        playerStats.Damage += _statsBoost.Damage;
//        playerStats.AttackSpeed += _statsBoost.AttackSpeed;
//        playerStats.Speed += _statsBoost.Speed;
//        playerStats.Range += _statsBoost.Range;
//        playerStats.MaxHealth += _statsBoost.MaxHealth;
//    }
//}
