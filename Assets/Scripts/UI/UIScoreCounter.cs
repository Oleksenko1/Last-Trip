using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreCounter : MonoBehaviour
{
    [Tooltip("Delay between adding score to a player")]
    [SerializeField] float delay = 0.5f;

    private Text scoreText;
    private int scoreAmount;
    private bool playerAlive = true;
    private DifficultyMultiplierScript difficultyMultiplier;

    private void Awake()
    {
        scoreText = GetComponent<Text>();
        scoreAmount = 0;
        difficultyMultiplier = GameObject.Find("DifficultyMultiplier").GetComponent<DifficultyMultiplierScript>();
    }
    private void Start()
    {
        StartCoroutine(ScoreCountCoroutine());
    }
    IEnumerator ScoreCountCoroutine() // Coroutine for counting score
    {
        while (playerAlive)
        {
            Debug.Log("+");
            AddScore(1);
            yield return new WaitForSeconds(delay / difficultyMultiplier.GetMultiplier());
        }
    }
    public void AddScore(int x)
    {
        scoreAmount += x;
        scoreText.text = scoreAmount.ToString();
    }
    public void StopCounting() // StopCounting when player dead
    {
        Debug.Log("Count stopped");
        playerAlive = false;
    }

    public int GetScore()
    {
        return scoreAmount;
    }
}
