using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPauseMenu : MonoBehaviour
{
    
    [SerializeField] GameObject pauseMenuPanel;
    [SerializeField] SoundSettingsScript soundSettings;
    private bool isPaused = false;
    private bool isPlayerAlive = true;
    private bool inCutscene = false;

    public void PauseGame() // Pauses the game. Needs to be atached to a player input event on pressing ESC button
    {
        if (isPaused == false && isPlayerAlive && inCutscene == false)
        {
            soundSettings.InMenu();
            Time.timeScale = 0;
            isPaused = true;
            pauseMenuPanel.SetActive(true);
        }
    }
    public void ResumeGame() // Resumes the game. Needs to be atached to a RESUME button in pause menu
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
        soundSettings.StartPlayingMusic();
        soundSettings.PlayBasicUiSound();
    }
    public void PlayerDied()
    {
        isPlayerAlive = false;
    }
    public void PlayerIsInCutscene(bool b) // Is player in cutscene?
    {
        inCutscene = b;
    }
}
