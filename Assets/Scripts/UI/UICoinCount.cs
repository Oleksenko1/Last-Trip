using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICoinCount : MonoBehaviour
{
    int total = 0;
    Text coinText;
    private void Awake()
    {
        coinText = GetComponent<Text>();
        coinText.text = total + "";
    }
    public void AddCoins(int x)
    {
        total += x;
        coinText.text = total + ""; 
    }
    public int GetTotalCoins()
    {
        return total;
    }
}
