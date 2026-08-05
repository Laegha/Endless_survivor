using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TimescaleManager : MonoBehaviour
{
    static TimescaleManager instance;
    public static TimescaleManager tm { get { return instance; } }
    List<TimescaleChangeInfo> _pendingChanges = new();
    //bool _onHitstop = false;
    //const float _hitstopCooldown = 1;
    //float _hitstopCooldownTimer;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += ResetTimescale;
    }

    void Update()
    {
        //ReduceHitstopCooldown();
        if (_pendingChanges.Count == 0)
            return;
        List<TimescaleChangeInfo> pendingChangesCopy = new List<TimescaleChangeInfo>(_pendingChanges);
        var fullStopChanges = pendingChangesCopy.Where(change => change.NewTimescale == 0).ToList();
        if (fullStopChanges.Count > 0)
        {
            foreach(var change in fullStopChanges)
            {
                if (!change.HasEnded())
                    continue;
                RemoveTimescaleChange(change);
            }
            return;
        }

        foreach (var change in pendingChangesCopy)
        {
            if (!change.HasEnded())
                continue;
            RemoveTimescaleChange(change);
        }


    }
    //public void Hitstop(float hitstopDuration)
    //{
    //    if(_onHitstop) return;
    //    _onHitstop=true;
    //    TimescaleChangeInfo hitstopChangeInfo = new(0, true, hitstopDuration, () => _onHitstop = false);
    //    AddTimescaleChange(hitstopChangeInfo);
    //}
    //void ReduceHitstopCooldown()
    //{
    //    if (_hitstopCooldownTimer <= 0 || _onHitstop)
    //        return;
    //    _hitstopCooldownTimer -= Time.unscaledDeltaTime;
    //    _onHitstop = false;
    //}
    public TimescaleChangeInfo AddTimescaleChange(TimescaleChangeInfo info)
    {
        TimescaleChangeInfo addedChange = new(info);
        _pendingChanges.Add(addedChange);
        UpdateChanges();
        return addedChange;
    }
    public void RemoveTimescaleChange(TimescaleChangeInfo removedChange)
    {
        removedChange.OnChangeEnded?.Invoke();
        _pendingChanges.Remove(removedChange);
        UpdateChanges();
    }
    void UpdateChanges()
    {
        //if there are no elements set timescale to 1
        if (_pendingChanges.Count == 0)
        {
            Time.timeScale = 1;
            return;
        }
        //sort by lowest timescale
        _pendingChanges.Sort((a, b) => a.NewTimescale.CompareTo(b.NewTimescale));
        //use the lowest timescale
        Time.timeScale = _pendingChanges[0].NewTimescale;
        
    }
    void ResetTimescale(Scene placeholderscene, LoadSceneMode placeholdermode)
    {
        Time.timeScale = 1;
        _pendingChanges = new();
    }
}
