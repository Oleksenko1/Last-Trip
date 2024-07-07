using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDownScript : MonoBehaviour
{
    [SerializeField] float speed;
    private DifficultyMultiplierScript difficulty;

    private void Awake()
    {
        difficulty = GameObject.Find("DifficultyMultiplier").GetComponent<DifficultyMultiplierScript>();
    }
    private void Update()
    {
        transform.Translate(Vector2.down * speed / 10 * Time.deltaTime * difficulty.GetMultiplier()); // Moves object - down
    }
}
