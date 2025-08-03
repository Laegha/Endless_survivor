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

    [HideInInspector] public List<CharacterData> unlockedCharacters;
    [HideInInspector] public List<WeaponData> unlockedWeapons;
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
    public void SetVolume(string volumeGroup, float volume)
    {
        _audioMixer.SetFloat(volumeGroup, Mathf.Log10(20) * volume);
    }
}
