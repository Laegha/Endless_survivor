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
        float _maxWaveCoins = 3;

        private void Start()
        {
            GameManager.gm.GachaCoins = 3;
        }
        public void DisplayMenu()
        {
            _menuObject.SetActive(true);
            _damageDealtDisplay.text = RunStatsManager.runStatsManager.totalDamageDealt + "";
            _killedEnemiesDisplay.text = RunStatsManager.runStatsManager.regularEnemiesKilled + "";
        }

        void CalculateEarnedCoins()
        {
            var wavesSurvived = RunStatsManager.runStatsManager.wavesSurvived;
            var waveCoins = Mathf.Floor(wavesSurvived / _waveCoinRelation);
            waveCoins = Mathf.Clamp(waveCoins, 0, _maxWaveCoins);
        }
    }