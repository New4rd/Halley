using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource boopAudioSource;
    [SerializeField] private List<AudioClip> musics;
    [SerializeField] private AudioClip boopSound;

    [SerializeField] private AudioClip endMusic;
    
    private List<AudioClip> usageClips;
    private int _currentTrack = 0;

    private float _basicMusicVolume;
    
    private bool _isPlaying = false;
    private bool _fadeOutMusic = false;
    
    private void Awake()
    {
        Instance = this;
        ShuffleTracks();
        _basicMusicVolume = musicAudioSource.volume;
    }

    private void Update()
    {
        if (!_isPlaying) return;
        
        if (!musicAudioSource.isPlaying)
        {
            
            if (musicAudioSource.clip == endMusic)
            {
                ShuffleTracks();
                musicAudioSource.clip = usageClips[0];
                musicAudioSource.Play();
                _currentTrack++;
            }
            
            else
            {
                musicAudioSource.clip = usageClips[_currentTrack];
                musicAudioSource.Play();

                _currentTrack++;
                if (_currentTrack == usageClips.Count)
                    _currentTrack = 0;
            }
        }
        
        if (_fadeOutMusic)
        {
            musicAudioSource.volume -= .1f;
        }
    }

    public IEnumerator StartDecreasingVolume(Action onDecreaseFinished)
    {
        _fadeOutMusic = true;

        yield return new WaitUntil(() => musicAudioSource.volume <= 0);
        _fadeOutMusic = false;
        ResetMusicVolume();
        onDecreaseFinished.Invoke();
    }

    private void ResetMusicVolume() => musicAudioSource.volume = _basicMusicVolume;

    public void StartPlaying()
    {
        _isPlaying = true;
    }

    public void PlayBoop()
    {
        boopAudioSource.PlayOneShot(boopSound);
    }

    public void StartPlayingOutroMusic()
    {
        musicAudioSource.clip = endMusic;
        musicAudioSource.Play();
    }
    
    private void ShuffleTracks()
    {
        usageClips = musics.OrderBy(x => Random.value).ToList();
    }
}
