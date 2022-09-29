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
    private String curSceneName = "";

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        

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
        String sceneName = SceneManager.GetActiveScene().name;
        if(curSceneName != sceneName)
        {
            curSceneName = sceneName;
            SwitchMusic();
        }
        if (Instance == null)
        {
            Instance = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }

    private void SwitchMusic()
    { 
        switch (curSceneName)
        {
            case "TaneimTesting":
                Play(sounds[0], source);
                break;
            case "MainMenu":
                Play(sounds[1], source);
                break;
            case "Tutorial":
                Play(sounds[2], source);
                break;
            case "Level1":
                Play(sounds[3], source);
                break;
            case "Level2":
                Play(sounds[4], source);
                break;
            case "Level3":
                Play(sounds[5], source);
                break;
            case "BossLevel":
                Play(sounds[6], source);
                break;
            case "Credits":
                Play(sounds[7], source);
                break;
            default:
                source.Stop();
                break;
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