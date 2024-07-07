using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMiniMap : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] Image icon;
    //[SerializeField] GameObject miniMap;
    [Tooltip("Target position of player icon. 0 - Start of minimap, 1 - To level 2, 2 - To level 3 ")]
    [SerializeField] Transform[] targetPos;
    [Header("Characteristics")]
    //[Tooltip("From where does a player icon starts")]
    //[SerializeField] float startPos;
    [Tooltip("Time length of single update of an icon")]
    [SerializeField] float step;

    private int currentTargetIndex = 1;
    private float totalSteps = 0;
    private bool isAlive = true;
    private float[] levelLength;
    public void Start()
    {
        levelLength = GameObject.Find("LevelChanger").GetComponent<LevelChanger>().GetLevelLength(); // Sets level length
        icon.transform.position = targetPos[0].transform.position;
        StartCoroutine(CountIconPosition());
    }
    IEnumerator CountIconPosition()
    {
        totalSteps += step;
        icon.transform.position = new Vector2(icon.transform.position.x, Mathf.Lerp(targetPos[currentTargetIndex - 1].position.y, targetPos[currentTargetIndex].position.y, totalSteps / levelLength[currentTargetIndex - 1]));
        yield return new WaitForSeconds(step);
        if (isAlive)
        {
            StartCoroutine(CountIconPosition());
        }
    }
    public void SetTargetLevel(int x)
    {
        currentTargetIndex = x + 1;
        totalSteps = 0;
    }
    public void StopUpdating() // Needs to be conected to a player death event
    {
        isAlive = false;
    }
}
