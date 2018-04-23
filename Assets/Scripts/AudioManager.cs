using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] clips;

    private AudioSource _source;

    private static AudioManager _audioManager;

    void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    void PlayClip(GameAudioClip clip)
    {
        if (_source.isPlaying)
        {
            _source.Stop();
        }

        _source.clip = clips[(int)clip];
        _source.Play();
    }

    public static void Play(GameAudioClip clip)
    {
        if (_audioManager == null)
        {
            _audioManager = GameObject.FindObjectOfType<AudioManager>();
        }

        _audioManager.PlayClip(clip);
    }
}

public enum GameAudioClip
{
    Jump,
    Hurt,
    Hurt2,
    Clock,
    ChangeTurn,
    ChangeTurn2
}
