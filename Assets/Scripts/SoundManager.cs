using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    
    
    
    [SerializeField] private Sound[] SoundsArray;
    private Dictionary<string, Sound> SoundsDictionary = new Dictionary<string, Sound>();
    public float Volume { get; private set; } = 0.5f;
    

    private void Awake()
    {
     
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        InitializeSounds(SoundsArray, SoundsDictionary);
        
    }

    private void InitializeSounds(Sound[] soundsArray, Dictionary<string, Sound> dictionary)
    {
        foreach (Sound s in soundsArray)
        {
            if (dictionary.ContainsKey(s.name))
            {
                Debug.LogError($"Звук с именем {s.name} уже существует!");
                continue;
            }

            dictionary.Add(s.name, s);
            
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            
        }
    }

    public void Play(string name)
    {
        if (SoundsDictionary.TryGetValue(name, out Sound sound))
        {
            sound.source.Play();
            Debug.LogWarning($"Music '{name}'is started ");
        }
        else
        {
            Debug.LogWarning($"Music '{name}' not found!");
        }
    }
    
    public void Stop(string name)
    {
        if (SoundsDictionary.TryGetValue(name, out Sound sound))
        {
            sound.source.Stop();
            Debug.LogWarning($"Music '{name}'is stopped ");
        }
        else
        {
            Debug.LogWarning($"Music '{name}' not found!");
        }
    }

 
    public void SetVolume(float volume)
    {
        Volume = volume;
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20;
        
    }


}