using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] GameObject _menuObject;
    [SerializeField] TextMeshProUGUI _damageDealtDisplay;
    [SerializeField] TextMeshProUGUI _killedEnemiesDisplay;

    float _waveCoinRelation = 10;
    int _maxWaveCoins = 3;
    int _earnedWaveCoins = 0;

    float _damageCoinRelation = 1000;
    int _maxDamageCoins = 3;
    int _earnedDamageCoins = 0;

    float _killCoinRelation = 100;
    int _maxKillCoins = 2;
    int _earnedKillCoins = 0;
    public void DisplayMenu()
    {
        _menuObject.SetActive(true);
        _damageDealtDisplay.text = RunStatsManager.runStatsManager.totalDamageDealt + "";
        _killedEnemiesDisplay.text = RunStatsManager.runStatsManager.regularEnemiesKilled + "";
    }

    void CalculateEarnedCoins()
    {
        var wavesSurvived = RunStatsManager.runStatsManager.wavesSurvived;
        _earnedWaveCoins = (int)Mathf.Floor(wavesSurvived / _waveCoinRelation);
        _earnedWaveCoins = Mathf.Clamp(_earnedWaveCoins, 0, _maxWaveCoins);

        var totalDamage = RunStatsManager.runStatsManager.totalDamageDealt;
        _earnedDamageCoins = (int)Mathf.Floor(totalDamage / _damageCoinRelation);
        _earnedDamageCoins = Mathf.Clamp(_earnedDamageCoins, 0, _maxDamageCoins);
        
        var kills= RunStatsManager.runStatsManager.regularEnemiesKilled;
        _earnedKillCoins = (int)Mathf.Floor(kills/ _killCoinRelation);
        _earnedKillCoins = Mathf.Clamp(_earnedKillCoins, 0, _maxKillCoins);
    }
}