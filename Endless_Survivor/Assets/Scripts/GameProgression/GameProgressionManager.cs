using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressionManager : MonoBehaviour
{
    static GameProgressionManager instance;
    public static GameProgressionManager gpm
    {
        get
        {
            return instance;
        }
    }
    int _totalBosses = -1;
    int _killedBosses = 0;
    public void BiomeGenerated(int addedBosses)
    {
        _totalBosses += addedBosses;
    }
    public void BossKilled()
    {
        _killedBosses++;
        if (_killedBosses >= _totalBosses)
            Debug.Log("KILLED ALL BOSSES");
    }
}
