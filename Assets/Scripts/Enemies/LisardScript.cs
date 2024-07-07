using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LisardScript : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    private DifficultyMultiplierScript difficultyMultiplier;
    private void Awake()
    {
        difficultyMultiplier = GameObject.Find("DifficultyMultiplier").GetComponent<DifficultyMultiplierScript>();
    }
    private void Update()
    {
        float totalSpeed = moveSpeed / 10;
        transform.Translate(Vector3.down * Time.deltaTime * totalSpeed);
    }
}
