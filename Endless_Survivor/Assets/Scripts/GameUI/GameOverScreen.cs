using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] GameObject _menuObject;
    [SerializeField] TextMeshProUGUI _damageDealtDisplay;
    [SerializeField] TextMeshProUGUI _killedEnemiesDisplay;
    [SerializeField] TextMeshProUGUI _wavesSurvivedDisplay;

    [SerializeField] TextMeshProUGUI _totalEarnedCoinDisplay;
    [SerializeField] TextMeshProUGUI _damageDealtCoinDisplay;
    [SerializeField] TextMeshProUGUI _killedEnemiesCoinDisplay;
    [SerializeField] TextMeshProUGUI _wavesSurvivedCoinDisplay;

    float _waveCoinRelation = 10;
    int _maxWaveCoins = 3;
    int _earnedWaveCoins = 0;

    float _damageCoinRelation = 10000;
    int _maxDamageCoins = 3;
    int _earnedDamageCoins = 0;

    float _killCoinRelation = 100;
    int _maxKillCoins = 2;
    int _earnedKillCoins = 0;

    int _totalEarnedCoins = 0;
    private void Start()
    {
        CalculateEarnedCoins();
        DisplayMenu();
    }
    public void DisplayMenu()
    {
        _menuObject.SetActive(true);
        _damageDealtDisplay.text = RunStatsManager.runStatsManager.totalDamageDealt + "";
        _killedEnemiesDisplay.text = RunStatsManager.runStatsManager.regularEnemiesKilled + "";
        _wavesSurvivedDisplay.text = RunStatsManager.runStatsManager.intensityLevelsSurvived + "";

        _totalEarnedCoinDisplay.text = "x" + _totalEarnedCoins;
        _wavesSurvivedCoinDisplay.text = "x" + _earnedWaveCoins;
        _damageDealtCoinDisplay.text = "x" + _earnedDamageCoins;
        _killedEnemiesCoinDisplay.text = "x" + _earnedKillCoins;
    }

    void CalculateEarnedCoins()
    {
        var wavesSurvived = RunStatsManager.runStatsManager.intensityLevelsSurvived;
        _earnedWaveCoins = (int)Mathf.Floor(wavesSurvived / _waveCoinRelation);
        _earnedWaveCoins = Mathf.Clamp(_earnedWaveCoins, 0, _maxWaveCoins);

        var totalDamage = RunStatsManager.runStatsManager.totalDamageDealt;
        _earnedDamageCoins = (int)Mathf.Floor(totalDamage / _damageCoinRelation);
        _earnedDamageCoins = Mathf.Clamp(_earnedDamageCoins, 0, _maxDamageCoins);
        
        var kills= RunStatsManager.runStatsManager.regularEnemiesKilled;
        _earnedKillCoins = (int)Mathf.Floor(kills/ _killCoinRelation);
        _earnedKillCoins = Mathf.Clamp(_earnedKillCoins, 0, _maxKillCoins);

        var totalEarnedCoins = _earnedDamageCoins + _earnedKillCoins + _earnedWaveCoins;
        UnlockmentsManager.AddGachaCoins(totalEarnedCoins);
        _totalEarnedCoins = totalEarnedCoins;

    }
}