using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIFinalScreen : MonoBehaviour
{
    [SerializeField] GameObject finalScreen;
    [SerializeField] GameObject mainUI;
    [Tooltip("Text field that count from 0 to a final score")]
    [SerializeField] Text mainScore;
    [SerializeField] GameObject highestScoreText;
    [SerializeField] GameObject coinsCollectedText;
    [SerializeField] float countSpeed;
    [Tooltip("Delay between player death and final panel appearing")]
    [SerializeField] float waitForAppear;
    [Tooltip("Delay between Highscore and Coins collected Text - appearing")]
    [SerializeField] float delayScoreCoins;
    [SerializeField] SoundSettingsScript soundSettings;
    [Tooltip("Delay between pressing restart/menu and then doing something")] 
    [SerializeField] float restartAndMenuDelay;

    int currentScore;
    int highestScore;
    int coinsCollected;
    int totalCoins;

    bool isRecord = false;
    private int temp = 0;
    private float delay;

    public void PlayerDied() // Needs to be atached to player death event. 1
    {
        highestScore = PlayerPrefs.GetInt("HighestScore", 0);
        currentScore = mainUI.GetComponentInChildren<UIScoreCounter>().GetScore();

        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0); 
        coinsCollected = mainUI.GetComponentInChildren<UICoinCount>().GetTotalCoins();
        PlayerPrefs.SetInt("TotalCoins", totalCoins + coinsCollected); // Addcoins to total amount

        highestScoreText.SetActive(false);
        coinsCollectedText.SetActive(false);

        if (currentScore > highestScore)
        {
            PlayerPrefs.SetInt("HighestScore", currentScore);
            isRecord = true;
        }

        StartCoroutine(StartFinalScreen());
    }
    public IEnumerator StartFinalScreen() // Activates final screen after some delay. 2
    {
        mainUI.SetActive(false);
        delay = countSpeed / currentScore;
        yield return new WaitForSeconds(waitForAppear);

        finalScreen.SetActive(true);
        StartCoroutine(CountCoroutine());
    }
    public IEnumerator CountCoroutine() // Starts counting. 3
    {
        temp++;
        mainScore.text = temp + "";
        yield return new WaitForSeconds(delay);
        if(temp < currentScore)
        {
            StartCoroutine(CountCoroutine());
        }
        else
        {
            StartCoroutine(CountEnded());
        }
    }
    private IEnumerator CountEnded() // 4
    {
        highestScoreText.SetActive(true);
        highestScoreText.GetComponent<Text>().text = "Highest score: " + PlayerPrefs.GetInt("HighestScore");
        if(isRecord)
        {
            highestScoreText.GetComponent<Animator>().SetTrigger("New record");
            soundSettings.PlayHighScoreSound();
        }

        yield return new WaitForSeconds(delayScoreCoins); // Smooth delay between appearing highscore and coins collected

        coinsCollectedText.SetActive(true);
        coinsCollectedText.GetComponent<Text>().text = "Coins collected: " + coinsCollected;
    }
    public void RestartButton()
    {
        StopAllCoroutines();
        soundSettings.PlayBasicUiSound();
        StartCoroutine(RestartButtonDelay());
    }
    IEnumerator RestartButtonDelay()
    {
        yield return new WaitForSeconds(restartAndMenuDelay);
        SceneManager.LoadScene("PlayingScene");
    }

    public void MenuButton()
    {
        StopAllCoroutines();
        soundSettings.PlayBasicUiSound();
        StartCoroutine(MenuButtonDelay());
    }
    IEnumerator MenuButtonDelay()
    {
        yield return new WaitForSeconds(restartAndMenuDelay);
        SceneManager.LoadScene("MainMenu");
    }
}
