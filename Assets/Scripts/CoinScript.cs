using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField] int value;
    public int Collect()
    {
        Debug.Log("Coin collected");
        Destroy(gameObject);
        return value;
    }
}
