using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameProgressionManager : MonoBehaviour
{
    public enum RunEndType
    {
        Win,
        Lose
    }
    static GameProgressionManager instance;
    public static GameProgressionManager gpm
    {
        get
        {
            return instance;
        }
    }
    const float _endRunDelay = 1;
    bool _exitGenerated = false;
    int _totalBosses = 0;
    int _killedBosses = 0;
    private void Awake()
    {
        instance = this;
    }
    public void AddBosses(int addedBosses)
    {
        _totalBosses += addedBosses;
    }
    public void BossKilled()
    {
        if (_exitGenerated)
            return;
        _killedBosses++;
        if (_killedBosses >= _totalBosses)
        {
            _exitGenerated = true;
            //get a random position in the map
            var availableTiles = MapManager.mm.LoadedTiles.Where(x => !x.IsWall && x.TileSupportObj == null).ToList();
            Vector2 exitPosition = availableTiles[Random.Range(0, availableTiles.Count)].transform.position;
            //create the exit there (grab it from the WorldConfigData)
            Transform runExit = Instantiate(GameManager.gm.WorldConfig.RunExitPrefab, exitPosition, Quaternion.identity).transform;
            //display a UI message (grab the text from the WorldConfigData)
            GameUIManager.uiManager.DisplayUIMessage(GameManager.gm.WorldConfig.OnExitCreatedMessageInfo);
            //create a pointer (grab the icon from the WorldConfigData)
            var pointerColor = GameManager.gm.WorldConfig.ExitPointerColor;
            var pointerIcon = GameManager.gm.WorldConfig.ExitPointerIcon;
            GameUIManager.uiManager.PointerManager.AddPointer(runExit, pointerColor, pointerIcon);
        }
    }
    public void EndRun(CustomAnimation playerAnimation, ParticleSystem playerParticles, RunEndType endType)
    {
        Time.timeScale = 0;
        PlayerControl.pc.gameObject.SetActive(false);
        bool useAnim = playerAnimation != null && playerAnimation.Frames.Length > 0;
        if(useAnim)
        {
            AnimatedObjConfig playerAnimConfig = new(playerAnimation, PlayerControl.pc.transform.position, Quaternion.identity, playerAnimation.AnimDuration);
            var exitCharAnimator = AnimatedObjsManager.aom.SpawnAnimatedObj(playerAnimConfig);
            //exitCharAnimator.AffectedByTimeScale = false;

        }
        if(playerParticles != null)
        {
            ParticleConfig playerParticlesConfig = new(playerParticles, PlayerControl.pc.transform.position, Quaternion.identity, playerParticles.main.duration);
            ParticleManager.pm.SpawnParticles(playerParticlesConfig);
        }
        float timeToEndRun =  _endRunDelay + (useAnim ? playerAnimation.AnimDuration : 0) + (playerParticles != null ? playerParticles.main.duration : 0);
        GameManager.gm.DelayActionRealtime(timeToEndRun, () => GameProgressionManager.gpm.LoadScreen(endType), null);
    }

    void LoadScreen(RunEndType endType)
    {
        switch(endType)
        {
            case RunEndType.Win:
                SceneLoadingFunctions.slf.Win();
                break;
            case RunEndType.Lose:
                SceneLoadingFunctions.slf.GameOver();
                break;
        }
    }
}
