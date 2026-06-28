using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource _musicSourcePrefab;
    [SerializeField] float _songTransitionDuration;
    AudioSource _currentMusicSource;
    SongTransitionController _currTransitionController;

    static MusicManager _instance;
    public static MusicManager mm
    {
        get { return _instance; }
    }
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += CreateMusicSource;
    }
    private void Update()
    {
        if(_currTransitionController == null) 
            return;
        bool transitionFinished = _currTransitionController.UpdateTransition();
        if (transitionFinished)
            _currTransitionController = null;
    }

    void CreateMusicSource(Scene placeholderscene, LoadSceneMode placeholdermode)
    {
        _currTransitionController = null;
        _currentMusicSource = GameObject.Instantiate(_musicSourcePrefab, Camera.main.transform);
    }

    public void ChangeMusic(AudioClip song, float volume = 1)
    {
        float transitionSpeed = (_currentMusicSource.volume + volume) / _songTransitionDuration;
        _currTransitionController = new(_currentMusicSource, song, transitionSpeed, volume);
    }

}

class SongTransitionController
{
    AudioSource _musicSource;
    AudioClip _outSong;
    float _transitionSpeed;
    float _finalVolume;

    public SongTransitionController(AudioSource musicSource, AudioClip outSong, float transitionSpeed, float finalVolume)
    {
        _musicSource = musicSource;
        _outSong = outSong;
        _transitionSpeed = transitionSpeed;
        _finalVolume = finalVolume;
    }

    public bool UpdateTransition()
    {
        float volumeIncrease = _transitionSpeed * Time.unscaledDeltaTime * (_musicSource.clip == _outSong  ? 1 : -1);
        _musicSource.volume += volumeIncrease;
        if(_musicSource.clip == _outSong)
        {
            if(_musicSource.volume >= _finalVolume)
            {
                _musicSource.volume = _finalVolume;
                return true;
            }
            return false;
        }
        if(_musicSource.volume <= 0)
        {
            _musicSource.volume = 0;
            _musicSource.clip = _outSong;
            _musicSource.Play();
            return false;
        }
        return false;
    }

}