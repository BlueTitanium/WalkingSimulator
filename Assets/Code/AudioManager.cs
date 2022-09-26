using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour
{
    [SerializeField] public GameManager Instance;
    [SerializeField] public AudioSource source;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup soundEffectsMixerGroup;
    [SerializeField] private Sound[] sounds;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        switch (SceneManager.GetActiveScene().name)
        {
            case "TaneimTesting":
                Play(sounds[0], source);
                break;
            default:
                break;
        }

        musicMixerGroup.audioMixer.SetFloat("Music", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        soundEffectsMixerGroup.audioMixer.SetFloat("SFX", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20);
        /*
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
            s.source.loop = s.isLoop;
            s.source.volume = s.volume;

            switch (s.audioType)
            {
                case Sound.AudioTypes.soundEffect:
                    s.source.outputAudioMixerGroup = soundEffectsMixerGroup;
                    break;

                case Sound.AudioTypes.music:
                    s.source.outputAudioMixerGroup = musicMixerGroup;
                    break;
            }

            if (s.playOnAwake)
                s.source.Play();
        }
        */
    }
    private void Update()
    {
        if(Instance == null)
        {
            Instance = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }
    public void Play(Sound s, AudioSource a)
    {
        if (s == null)
        {
            Debug.LogError("Sound does NOT exist!");
            return;
        }
        s.source = a;
        s.source.clip = s.audioClip;
        s.source.loop = s.isLoop;
        s.source.volume = s.volume;
        switch (s.audioType)
        {
            case Sound.AudioTypes.soundEffect:
                s.source.outputAudioMixerGroup = soundEffectsMixerGroup;
                break;

            case Sound.AudioTypes.music:
                s.source.outputAudioMixerGroup = musicMixerGroup;
                break;
        }
        s.source.Play();
    }

    public void Stop(Sound s, AudioSource a)
    {
        s.source.Stop();
    }

    public void UpdateMixerVolume()
    {
        print("updating");
        musicMixerGroup.audioMixer.SetFloat("Music", Mathf.Log10(Instance.musicVolume) * 20);
        soundEffectsMixerGroup.audioMixer.SetFloat("SFX", Mathf.Log10(Instance.soundEffectsVolume) * 20);
    }
}