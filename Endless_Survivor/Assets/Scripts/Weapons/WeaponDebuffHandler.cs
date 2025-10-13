using System.Collections.Generic;

class WeaponDebuffHandler
{
    public List<Weapon> buffedWeapons;
    public WeaponStats statsBuff;

    public WeaponDebuffHandler(List<Weapon> buffedWeapons, WeaponStats statsBuff)
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