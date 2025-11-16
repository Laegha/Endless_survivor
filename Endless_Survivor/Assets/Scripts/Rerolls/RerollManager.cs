using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RerollManager : MonoBehaviour
{
    List<Reroll> _currentRerolls = new();
    Action _onReroll;
    public Action OnReroll { get { return _onReroll; } set { _onReroll = value; } }
    public int RerollsLeft 
    {
        get
        {
            int rollsLeft = 0;
            foreach(var roll in _currentRerolls)
            {
                rollsLeft+= roll.rerollsLeft;
            }
            return rollsLeft;
        }
    }
    static RerollManager instance;
    public static RerollManager rm {  get { return instance; } }
    private void Awake()
    {
        instance = this;
    }
    public bool ContainsRoll(Reroll roll)
    {
        return _currentRerolls.Contains(roll);
    }
    public void AddReroll(Reroll reroll)
    {
        _currentRerolls.Add(reroll);
        _currentRerolls.Sort((rerollA, rerollB) => rerollA.usePriority.CompareTo(rerollB.usePriority));//sorts from lowest to highest priority

    }

    public void UseReroll()
    {
        var usedReroll = _currentRerolls.Last();
        usedReroll.rerollsLeft--;
    }
}
