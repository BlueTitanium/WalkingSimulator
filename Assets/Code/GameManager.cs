using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject DeathScreen;
    public CustomPlayerController player;
    public float musicVolume;
    public float soundEffectsVolume;
    public float XSens;
    public float YSens;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundEffectsSlider;
    [SerializeField] private TextMeshProUGUI musicSliderText;
    [SerializeField] private TextMeshProUGUI soundEffectsSliderText;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup soundEffectsMixerGroup;
    [SerializeField] private TMP_InputField XSensText;
    [SerializeField] private TMP_InputField YSensText;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        DeathScreen.SetActive(false);
        player.paused = false;
        Time.timeScale = 1f;
        GameObject[] ams = GameObject.FindGameObjectsWithTag("Music");
        StartCoroutine(LoadOptions());
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pauseMenu != null)
        {
            if (pauseMenu.activeInHierarchy)
            {
                ClosePauseMenu();
            } else
            {
                ShowPauseMenu();
            }
        }
    }

    public void SaveOptions()
    {
        PlayerPrefs.SetFloat("XSens", XSens);
        PlayerPrefs.SetFloat("YSens", YSens);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", soundEffectsVolume);
        PlayerPrefs.Save();
        //if (player != null)
        //{
        //    player.lookSpeedX = XSens;
        //    player.lookSpeedY = YSens;
        //}
    }

    public IEnumerator LoadOptions()
    {
        yield return new WaitForSeconds(0f);
        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            SaveOptions();
        }
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        soundEffectsVolume = PlayerPrefs.GetFloat("SFXVolume");
        XSens = PlayerPrefs.GetFloat("XSens");
        YSens = PlayerPrefs.GetFloat("YSens");

        musicSlider.value = musicVolume;
        musicSliderText.text = ((int)(musicVolume * 100)).ToString();
        soundEffectsSlider.value = soundEffectsVolume;
        soundEffectsSliderText.text = ((int)(soundEffectsVolume * 100)).ToString();
        XSens = PlayerPrefs.GetFloat("XSens");
        YSens = PlayerPrefs.GetFloat("YSens");
        EditSensText();
        if(player != null)
        {
            player.lookSpeedX = XSens;
            player.lookSpeedY = YSens;
        } 
    }

    public void UpdateMixerVolume()
    {
        musicMixerGroup.audioMixer.SetFloat("Music", Mathf.Log10(musicVolume) * 20);
        soundEffectsMixerGroup.audioMixer.SetFloat("SFX", Mathf.Log10(soundEffectsVolume) * 20);
    }

    public void ShowDeathScreen()
    {
        DeathScreen.SetActive(true);
        player.paused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ShowPauseMenu()
    {
        player.paused = true;
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void ShowOptionsMenu()
    {
        optionsMenu.SetActive(true);
    }

    public void CloseDeathScreen()
    {
        DeathScreen.SetActive(false);
        player.paused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void ClosePauseMenu()
    {
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        player.paused = false;
        Cursor.lockState = CursorLockMode.Locked;
        SaveOptions();
    }
    public void CloseOptionsMenu()
    {
        SaveOptions();
        optionsMenu.SetActive(false);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }



    public void OnMusicSliderValueChange(float value)
    {
        musicVolume = value;

        musicSliderText.text = ((int)(value * 100)).ToString();
        UpdateMixerVolume();
        SaveOptions();
    }

    public void OnSoundEffectsSliderValueChange(float value)
    {
        soundEffectsVolume = value;

        soundEffectsSliderText.text = ((int)(value * 100)).ToString();
        UpdateMixerVolume();
        SaveOptions();
    }

    public void OnEndEditXSens(string value)
    {
        float v = float.Parse(value);
        if(v < 0.01f)
        {
            v = .01f;
        }
        if(v > 10f)
        {
            v = 10f;
        }
        XSens = v;
        EditSensText();
        SaveOptions();
    }
    public void OnEndEditYSens(string value)
    {
        float v = float.Parse(value);
        if (v < 0.01f)
        {
            v = .01f;
        }
        if (v > 10f)
        {
            v = 10f;
        }
        YSens = v;
        EditSensText();
        SaveOptions();
    }
    public void EditSensText()
    {
        XSensText.text = XSens + "";
        YSensText.text = YSens + "";
    }

}
