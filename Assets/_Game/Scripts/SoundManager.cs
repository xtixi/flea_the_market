using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> coinSounds;
    [SerializeField] private AudioSource AudioSource;

    public static SoundManager instance;
    
    private void Awake()
    {
        instance = this;
    }

    public void PlayCoinSound()
    {
        AudioSource.PlayOneShot(coinSounds[Random.Range(0,coinSounds.Count)]);
    }
}
