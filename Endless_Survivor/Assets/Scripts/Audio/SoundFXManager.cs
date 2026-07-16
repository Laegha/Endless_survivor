using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    static SoundFXManager instance;
    public static SoundFXManager sm {  get { return instance; } }
    [SerializeField] AudioSource _sfxSourcePrefab;
    private void Awake()
    {
        instance = this;
    }

    public GameObject PlaySfx(AudioClip sound, float volume, float minPitchVariation, float maxPitchVariation, Vector2 position)
    {
        if (sound == null)
            return null;
        AudioSource audioSource = Instantiate(_sfxSourcePrefab, position, Quaternion.identity);
        audioSource.clip = sound;
        audioSource.volume = volume;
        audioSource.pitch += Random.Range(minPitchVariation, maxPitchVariation);
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
        return audioSource.gameObject;
    }
    public GameObject PlaySfx(SFXInfo soundInfo, Vector2 position)
    {
        if(soundInfo == null) return null;
        return PlaySfx(soundInfo.Sound, soundInfo.Volume, soundInfo.MinPitchVariation, soundInfo.MaxPitchVariation, position);
    }
}
