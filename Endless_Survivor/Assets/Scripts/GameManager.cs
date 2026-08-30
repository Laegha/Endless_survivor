using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CharacterData selectedCharacter;

    public PrefabHolder prefabHolder;
    [SerializeField] SettingsHandler _settingsHandler;
    [SerializeField] MapGenerationConfig _mapGenerationConfig;
    [SerializeField] WorldConfigData _worldConfig;
    [SerializeField] WeaponStatsSpritesData _weaponStatsSprites;
    UnlockedElementsHelper _unlockedElementsHelper = new();

    public SettingsHandler SettingsHandler { get { return _settingsHandler; } }
    public MapGenerationConfig MapGenerationConfig { get { return _mapGenerationConfig; } }
    public WorldConfigData WorldConfig { get { return _worldConfig; } }
    public WeaponStatsSpritesData WeaponStatsSprites { get { return _weaponStatsSprites; } }
    public UnlockedElementsHelper UnlockedElementHelper {  get { return _unlockedElementsHelper; } }   

    public static GameManager gm
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        _settingsHandler.GetJsonValues();
        _unlockedElementsHelper.UpdateAll();
    }

    public void RoutineRunner(IEnumerator routine)
    {
        StartCoroutine(routine);
    }
    public void DelayAction(float delay, Action action, Func<bool> abortCondition)
    {
        StartCoroutine(CallDelayedAction(delay, action, abortCondition));
    }
    public void DelayActionAFrame(Action action, Func<bool> abortCondition)
    {
        StartCoroutine(CallDelayedAFrameAction(action, abortCondition));
    }
    public void DelayActionRealtime(float delay, Action action, Func<bool> abortCondition)
    {
        StartCoroutine(CallDelayedActionRealtime(delay, action, abortCondition));
    }
    IEnumerator CallDelayedAction(float delay, Action action, Func<bool> abortCondition)
    {
        yield return new WaitForSeconds(delay);
        if(abortCondition != null && abortCondition())
            yield break;
        action?.Invoke();
    }
    IEnumerator CallDelayedAFrameAction(Action action, Func<bool> abortCondition)
    {
        yield return new WaitForEndOfFrame();
        if (abortCondition != null && abortCondition())
            yield break;
        action?.Invoke();
    }
    IEnumerator CallDelayedActionRealtime(float delay, Action action, Func<bool> abortCondition)
    {
        yield return new WaitForSecondsRealtime(delay);
        if (abortCondition != null && abortCondition())
            yield break;
        action?.Invoke();
    }
}
