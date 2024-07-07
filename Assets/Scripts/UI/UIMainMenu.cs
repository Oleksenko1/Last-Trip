using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] GameObject settingsCanvas;
    [SerializeField] GameObject creditCanvas;
    [SerializeField] GameObject exitCanvas;
    [SerializeField] GameObject blackScreen;
    [SerializeField] SoundSettingsScript soundSettings;
    [Tooltip("Delay between clicking close/open and activating script. Needed so sounds have time to play")] 
    [SerializeField] float playAndExitDelay;

    private void Start()
    {
        settingsCanvas.SetActive(false);
        creditCanvas.SetActive(false);
        exitCanvas.SetActive(false);

        soundSettings.StartPlayingMusic();
    }
    public void SettingsIsOpened(bool b)
    {
        settingsCanvas.SetActive(b);
        BasicUiSounds(b);
    }
    public void CreditsIsOpened(bool b)
    {
        creditCanvas.SetActive(b);
        BasicUiSounds(b);
    }
    public void ExitIsOpened(bool b)
    {
        exitCanvas.SetActive(b);
        BasicUiSounds(b);
    }
    public void BasicUiSounds(bool b) // Plays basic ui sounds. Contains sounds: Open, Close
    {
        if (b)
        {
            soundSettings.PlayBasicUiSound();
        }
        else
        {
            soundSettings.PlayNegativeUiSound();
        }
    }


    public void ExitTheGame() // Exits the game
    {
        StartCoroutine(ExitTheGameAfterDelay(playAndExitDelay));
        soundSettings.PlayNegativeUiSound();
        soundSettings.StopPlayingMusic();
    }
    public IEnumerator ExitTheGameAfterDelay(float wait)
    {
        OpenBlackScreen();
        yield return new WaitForSeconds(wait);
        Application.Quit();
    }

    public void PlayPressed() // Starts the game
    {
        StartCoroutine(LoadGameAfterDelay(playAndExitDelay));
        soundSettings.PlayPlayButton();
        soundSettings.StopPlayingMusic();
    }
    public IEnumerator LoadGameAfterDelay(float wait)
    {
        OpenBlackScreen();
        yield return new WaitForSeconds(wait);
        SceneManager.LoadScene("PlayingScene");
    }

    public void OpenBlackScreen() // Opens slowly fading black screen
    {
        blackScreen.SetActive(true);
    }
}
