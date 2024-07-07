using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Range(0, 10)]
    [SerializeField] float scrollSpeed;
    [SerializeField] public float offsetX;
    private float offset;
    private Material mat;
    private DifficultyMultiplierScript difficulty;
    private void Awake()
    {
        difficulty = GameObject.Find("DifficultyMultiplier").GetComponent<DifficultyMultiplierScript>();

        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        offset += (Time.deltaTime * scrollSpeed * difficulty.GetMultiplier()) / 10f;
        mat.SetTextureOffset("_MainTex", new Vector2(offsetX, offset));
    }
}
