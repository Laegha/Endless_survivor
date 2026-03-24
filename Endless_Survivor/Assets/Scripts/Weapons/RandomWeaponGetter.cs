using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class RandomWeaponGetter
{
    static int _sharedTagChanceIncrementBase = 10;
    static int _sharedTagChanceIncrementPerTag = 1;
    public static ElementIsNewInfo<WeaponData> GetWeapon(bool useBuildTags = true, CustomFlags.IWeaponPool weaponPool = CustomFlags.IWeaponPool.None)
    {
        var availableWeapons = GameManager.gm.UnlockedElementHelper.UnlockedWeapons;
        if(weaponPool != CustomFlags.IWeaponPool.None)
        {
            availableWeapons = availableWeapons.Where(x => (x.element.WeaponPools & weaponPool) != CustomFlags.IWeaponPool.None).ToList();
        }

        if(!useBuildTags)
        {
            return availableWeapons[Random.Range(0, availableWeapons.Count)];
        }
        var heldTags = PlayerControl.pc != null ? PlayerControl.pc.WeaponManager.HeldWeaponTags : new();
        Dictionary<ElementIsNewInfo<WeaponData>, int> rouletteMaterial = new Dictionary<ElementIsNewInfo<WeaponData>, int>();
        foreach(var weaponData in availableWeapons)
        {
            rouletteMaterial.Add(weaponData, 10);
        }

        foreach(var weaponElementInfo in availableWeapons)
        {
            var sharedTags = weaponElementInfo.element.WeaponTags.Intersect(heldTags.Keys).Count();
            var chanceIncrement = sharedTags == 0 ? 0 : _sharedTagChanceIncrementBase + sharedTags * _sharedTagChanceIncrementPerTag - 1;
            rouletteMaterial[weaponElementInfo] += chanceIncrement;
        }
        Roulette<ElementIsNewInfo<WeaponData>> weaponRoulette = new(rouletteMaterial);
        return weaponRoulette.Spin();
    }
}
