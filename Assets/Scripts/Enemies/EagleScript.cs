using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleScript : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    private DifficultyMultiplierScript difficultyMultiplier;
    private void Awake()
    {
        difficultyMultiplier = GameObject.Find("DifficultyMultiplier").GetComponent<DifficultyMultiplierScript>();
    }
    private void Update()
    {
        float totalSpeed = (moveSpeed + moveSpeed * difficultyMultiplier.GetMultiplier()) / 10;
        transform.Translate(Vector3.up * Time.deltaTime * totalSpeed);
    }
}
