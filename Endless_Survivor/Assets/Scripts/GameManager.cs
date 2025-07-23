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
    static string unlockmentsFilePath = Path.Combine(Application.streamingAssetsPath, unlockmentsFileName);
    static string unlockmentsJson = File.ReadAllText(unlockmentsFilePath);
    [HideInInspector] public List<CharacterData> unlockedCharacters;
    [HideInInspector] public List<WeaponData> unlockedWeapons;

    int _gachaCoins = 0;
    public int GachaCoins 
    { get { return _gachaCoins; } 
        set 
        {
            _gachaCoins = value;
            UnlockmentsConverter unlockments = JsonConvert.DeserializeObject<UnlockmentsConverter>(unlockmentsJson);
            unlockments.gachaCoins = value;
            string json = JsonConvert.SerializeObject(unlockments);
            File.WriteAllText(unlockmentsFilePath, json);

        }
    }

    [SerializeField] AudioMixer _audioMixer;

    [SerializeField] WeaponData _testWeapon;

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
       _testWeapon = Resources.Load("ScriptableObjects/Weapons/Balance") as WeaponData;
    }
    public UnlockmentsConverter Unlockments 
    {
        get
        {
            string path = Path.Combine(Application.streamingAssetsPath, unlockmentsFileName);
            string json = File.ReadAllText(path);
            UnlockmentsConverter unlockmentsData = JsonConvert.DeserializeObject<UnlockmentsConverter>(json);
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
