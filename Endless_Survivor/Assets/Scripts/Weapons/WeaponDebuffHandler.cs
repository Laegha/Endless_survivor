using System.Collections.Generic;

class WeaponDebuffHandler
{
    public List<WeaponAttackManager> buffedWeapons;
    public WeaponStats statsBuff;

    public WeaponDebuffHandler(List<WeaponAttackManager> buffedWeapons, WeaponStats statsBuff)
    {
        this.buffedWeapons = buffedWeapons;
        this.statsBuff = statsBuff;
    }

    public void DebuffWeapons()
    {
        foreach (var weapon in buffedWeapons)
        {
            if (weapon == null) continue;
            weapon.WeaponStats.TemporalStatIncrease(statsBuff, true);
        }
    }
}