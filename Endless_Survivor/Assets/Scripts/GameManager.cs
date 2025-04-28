using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CharacterData selectedCharacter;

    public PrefabHolder prefabHolder;
    public UnlockmentsHolder unlockmentsHolder;
    static string unlockmentsFileName = "unlockments.json";
    [HideInInspector] public List<CharacterData> unlockedCharacters;
    [HideInInspector] public List<WeaponData> unlockedWeapons;
    [SerializeField] AudioMixer _audioMixer;

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
    public UnlockmentsInfo Unlockments 
    {
        get
        {
            string path = Path.Combine(Application.streamingAssetsPath, unlockmentsFileName);
            string json = File.ReadAllText(path);
            UnlockmentsInfo unlockmentsData = JsonConvert.DeserializeObject<UnlockmentsInfo>(json);
            return unlockmentsData;
        }
    }
    public void StartRun()
    {
        unlockedWeapons = unlockmentsHolder.UnlockedWeapons();
    }
    public void SetVolume(string volumeGroup, float volume)
    {
        _audioMixer.SetFloat(volumeGroup, Mathf.Log10(20) * volume);
    }
}
