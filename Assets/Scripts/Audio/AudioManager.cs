
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource fxSource;
    public AudioSource musicSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    public void PlaySound(AudioClip clip)
    {
        fxSource.clip = clip;
        fxSource.Play();
    }
}
