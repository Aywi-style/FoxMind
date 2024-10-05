using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private EventInstance _musicEventInstance;
    [SerializeField] private EventReference _bossBattle_1;
    
    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
        
        StartMusic();
    }

    private void StartMusic()
    {
        _musicEventInstance = CreateInstance(_bossBattle_1);
        _musicEventInstance.start();
    }

    private EventInstance CreateInstance(EventReference eventReference)
    {
        return RuntimeManager.CreateInstance(eventReference);
    }
}
