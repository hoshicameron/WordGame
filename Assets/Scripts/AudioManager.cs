using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private bool muteBackgroundMusic = false;
    private bool muteSoundFx = false;

    private AudioSource audioSource;
    public static AudioManager Instance { get; set; }

    private void Awake()
    {
        // Singleton Implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        } else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    public void ToggleBackgroundMusic()
    {

        muteBackgroundMusic = !muteBackgroundMusic;

        if (muteBackgroundMusic) audioSource.Stop();
        else                     audioSource.Play();
    }

    public void ToggleSoundFx()
    {
        muteSoundFx = !muteSoundFx;

        GameEvents.CallSoundFxEvent();
    }

    public bool IsBackgroundMusicMuted()
    {
        return muteBackgroundMusic;
    }

    public bool IsSoundFxMuted()
    {
        return muteSoundFx;
    }

    public void SilenceBackgroundMusic(bool silence)
    {
        if (muteBackgroundMusic == false)
        {
            audioSource.volume = (silence) ? 0f : 1f;
        }
    }

}
