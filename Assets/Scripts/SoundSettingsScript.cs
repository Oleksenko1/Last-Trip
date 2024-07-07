using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundSettingsScript : MonoBehaviour
{
    [Tooltip("AudioSource that will play UI sounds")]
    [SerializeField] AudioSource uiAS;
    [SerializeField] AudioSource musicAS;
    [SerializeField] AudioMixerGroup Mixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [Tooltip("Minimum volume that can be edited")]
    [Range(-80, 0)]
    [SerializeField] float minVolume;

    [Header("Sounds")]
    [Space(5)] [Range(0, 1)]
    [SerializeField] float basicUiVolume;
    [SerializeField] AudioClip basicUiAC;
    [Space(5)] [Range(0, 1)]
    [SerializeField] float playButtonVolume;
    [SerializeField] AudioClip playButtonAC;
    [Space(5)] [Range(0, 1)]
    [SerializeField] float negativeUiVolume;
    [SerializeField] AudioClip negativeUiAC;
    [Space(5)]
    [Range(0, 1)]
    [SerializeField] float highscoreUiVolume;
    [SerializeField] AudioClip highscoreUiAC;
    [Space(5)]
    [Range(0, 1)]
    [SerializeField] float playerColidedVolume;
    [SerializeField] AudioClip playerColidedAC;
    [Space(5)]
    [Range(0, 1)]
    [SerializeField] float playerStolenVolume;
    [SerializeField] AudioClip playerStolenAC;
    [Space(5)]
    [Range(0, 1)]
    [SerializeField] float coinCollectedVolume;
    [SerializeField] AudioClip[] coinCollectedAC;
    [Space(5)]
    [SerializeField] AudioClip oceanMusic;

    [Header("Sound effects")]
    [SerializeField] AudioMixerSnapshot inMenu;
    [SerializeField] AudioMixerSnapshot normal;
    [SerializeField] float transitionTime;
    [SerializeField] AudioMixerSnapshot turnOffMusic;

    private float masterVolume;
    private float musicVolume;
    private float sfxVolume;

    private void Awake()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolumeSettings", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolumeSettings", 0.78f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolumeSettings", 0.7f);

        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
    }
    private void Start()
    {
        ChangeMasterVolume(masterVolume);
        ChangeMusicVolume(musicVolume);
        ChangeSFXVolume(sfxVolume);

        StartPlayingMusic();
    }

    public void ChangeMasterVolume(float x)
    {
        Mixer.audioMixer.SetFloat("MasterVolume", Mathf.Lerp(minVolume, 0, x));
        PlayerPrefs.SetFloat("MasterVolumeSettings", x); // Saves volume settings
    }
    public void ChangeMusicVolume(float x)
    {
        Mixer.audioMixer.SetFloat("MusicVolume", Mathf.Lerp(minVolume, 0, x));
        PlayerPrefs.SetFloat("MusicVolumeSettings", x); // Saves volume settings
    }
    public void ChangeSFXVolume(float x)
    {
        Mixer.audioMixer.SetFloat("SFXVolume", Mathf.Lerp(minVolume, 0, x));
        PlayerPrefs.SetFloat("SFXVolumeSettings", x); // Saves volume settings
    }
    public void PlayBasicUiSound()
    {
        uiAS.PlayOneShot(basicUiAC, basicUiVolume);
    }
    public void PlayNegativeUiSound()
    {
        uiAS.PlayOneShot(negativeUiAC, negativeUiVolume);
    }
    public void PlayPlayButton()
    {
        uiAS.PlayOneShot(playButtonAC, playButtonVolume);
    }
    public void PlayHighScoreSound()
    {
        uiAS.PlayOneShot(highscoreUiAC, highscoreUiVolume);
    }

    public void StopPlayingMusic()
    {
        turnOffMusic.TransitionTo(transitionTime);
    }
    public void InMenu()
    {
        inMenu.TransitionTo(0f);
    }
    public void ChangeMusic(AudioClip temp)
    {
        musicAS.clip = temp;
        musicAS.Play();
    }
    public void StartPlayingMusic()
    {
        normal.TransitionTo(transitionTime);
    }

    public void PlayPLayerColided()
    {
        uiAS.PlayOneShot(playerColidedAC, playerColidedVolume);
    }
    public void PlayPlayerStolen()
    {
        uiAS.PlayOneShot(playerStolenAC, playerStolenVolume);
    }

    public void CoinPickedUp(int x)
    {
        AudioClip temp = null;
        switch (x)
        { 
            case 1:
                temp = coinCollectedAC[0];
                break;
            case 3:
                temp = coinCollectedAC[1];
                break;
            case 6:
                temp = coinCollectedAC[2];
                break;
        }
        uiAS.PlayOneShot(temp, coinCollectedVolume);
    }
}
