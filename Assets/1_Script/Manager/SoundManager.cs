using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource sound;
    public static SoundManager Instance { get; private set; }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);    
        }
    }

    public void SFXSound(string soundName,AudioClip clip)
    {
        GameObject newSound = new GameObject(soundName + "Sound");
        AudioSource scorce = newSound.AddComponent<AudioSource>();
        scorce.clip = clip;
        scorce.Play();

        Destroy(scorce, clip.length);
    }

    public void BGSound(AudioClip clip)
    {
        sound.clip = clip;
        sound.loop = true;
        sound.volume = 0.4f;
        sound.Play();
    }
}
