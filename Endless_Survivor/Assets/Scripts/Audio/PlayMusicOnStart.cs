using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicOnStart : MonoBehaviour
{
    [SerializeField] AudioClip _song;
    [SerializeField] float _volume = 1;
    void Start()
    {
        MusicManager.mm.ChangeMusic(_song, _volume);
    }
}
