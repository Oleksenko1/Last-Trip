using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyMultiplierScript : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] float difficultyMultiplier;
    [Tooltip("How often does a difficulty rise")]
    [SerializeField] float riseTime;
    [Tooltip("How much does difficulty rise")]
    [SerializeField] float riseAmount;
    [SerializeField] float maxLimit;
    bool isRising = true;

    private void Start()
    {
        StartCoroutine(DifficultyEncreasingCoroutine());
    }
    IEnumerator DifficultyEncreasingCoroutine()
    {
        while (isRising)
        {
            if (difficultyMultiplier < maxLimit)
            {
                difficultyMultiplier += riseAmount;
            }
            yield return new WaitForSeconds(riseTime);
        }
    }
    public float GetMultiplier() 
    {
        return difficultyMultiplier;
    }
    public void SetMultiplier(float x)
    {
        difficultyMultiplier = x;
    }
    public void StopEncreasing() // Sets - if rising is active
    {
        isRising = false;
    }
}
