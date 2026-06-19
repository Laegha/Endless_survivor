using System.Collections;
using System.Collections.Generic;
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
    int _totalBosses = 0;
    int _killedBosses = 0;
    private void Awake()
    {
        instance = this;
    }
    public void AddBoss()
    {
        _totalBosses ++;
    }
    public void BossKilled()
    {
        _killedBosses++;
        if (_killedBosses >= _totalBosses)
            Debug.Log("KILLED ALL BOSSES");
    }
    public void EndRun(CustomAnimation playerAnimation, RunEndType endType)
    {
        Time.timeScale = 0;
        PlayerControl.pc.gameObject.SetActive(false);
        AnimatedObjConfig playerAnimConfig = new(playerAnimation, PlayerControl.pc.transform.position, Quaternion.identity, playerAnimation.AnimDuration);
        var exitCharAnimator = AnimatedObjsManager.aom.SpawnAnimatedObj(playerAnimConfig);
        //exitCharAnimator.AffectedByTimeScale = false;
        float timeToEndRun = playerAnimation.AnimDuration + _endRunDelay;
        GameManager.gm.DelayAction(timeToEndRun, () => GameProgressionManager.gpm.LoadScreen(endType), null);
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
