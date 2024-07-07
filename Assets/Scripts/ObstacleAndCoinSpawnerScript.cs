using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAndCoinSpawnerScript : MonoBehaviour
{
    public enum LevelLocation
    {
        Beach = 0,
        Ocean = 1,
        DeepOcean = 2
    };

    [Header("Game objects")]
    [SerializeField] GameObject[] beachObstaclePrefab;
    [SerializeField] GameObject[] oceanObstaclePrefab;
    [Tooltip("Slot 1 - BASIC, slot 2 - MEDIUM, slot 3 - HIGH")]
    [SerializeField] GameObject[] coinPrefab;

    [Header("Position of spawn")]
    [SerializeField] float positionY = 9;
    [Tooltip("Range from zero to VAR where obstacles can spawn")]
    [SerializeField] float rangeX = 2.8f;
    [Tooltip("How much further (randomly) can obstacles spawn from original position on coordinate X")]
    [SerializeField] float offsetX = 0.5f;
    [Tooltip("How much higher (randomly) can obstacles spawn from original position on coordinate Y")]
    [SerializeField] float offsetY = 0.5f;
    [Tooltip("Needs to have size as number of SpawnPoints you want. Var fields leave empty")]
    [SerializeField] float[] spawnPointsX;

    [Header("Characteristics")]
    [Tooltip("How fast obstacles spawns")]
    [SerializeField] float quantitySpawn = 10;
    [Tooltip("Chance to spawn coin per obstacle spawn")]
    [SerializeField] int coinChance = 10;
    [Tooltip("Chance that spawned coin would be BASIC quality")]
    [SerializeField] int basicCoinChance = 70;
    [Tooltip("Chance that spawned coin would be MEDIUM quality")]
    [SerializeField] int mediumCoinChance = 25;
    [Tooltip("Chance that spawned coin would be HIGH quality")]
    [SerializeField] int highCoinChance = 5;
    [Tooltip("How much obstacles in one line can spawn on each level")]
    [SerializeField] int[] obstaclesAmount;

    private float posStep;
    private int spawnPointsCount;
    private LevelLocation currentLocation;

    private DifficultyMultiplierScript difficulty;
    bool doSpawn = true;
    private void Awake()
    {
        difficulty = GameObject.Find("DifficultyMultiplier").GetComponent<DifficultyMultiplierScript>();

        spawnPointsCount = spawnPointsX.Length;
        posStep = rangeX * 2 / (spawnPointsCount - 1);
        for(int x = 0; x < spawnPointsCount; x++) // Fills array with X position for obstacle to spawn
        {
            spawnPointsX[x] = -rangeX + posStep * x;
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnObstacleCoroutine()); // Starts spawning enemies
    }

    IEnumerator SpawnObstacleCoroutine() // Coroutine that spawns obstacles
    {
        while (doSpawn)
        {
            int x = 0;
            int timesToSpawn = Random.Range(1, obstaclesAmount[(int)currentLocation] + 1);
            Shuffle(spawnPointsX);
            for (x = 0 ; x < timesToSpawn; x++) // Loop for spawning more than one obstacle
            {
                Vector3 pos = new Vector3(spawnPointsX[x] + Random.Range(-offsetX, offsetX), positionY + Random.Range(0, offsetY), 0); // Creates random position
                Instantiate(GetRandomObstacle(), pos, Quaternion.Euler(0, 0 ,0));
            }

            SpawnCoin(spawnPointsX[x], positionY); // Chance to spawn coin

            yield return new WaitForSeconds(10 / (quantitySpawn * difficulty.GetMultiplier()));
        }
    }
    GameObject GetRandomObstacle() // Returns random obstacle
    {
        int rnd;
        switch(currentLocation)
        {
            case LevelLocation.Beach:
                rnd = Random.Range(0, beachObstaclePrefab.Length);
                return beachObstaclePrefab[rnd];
            case LevelLocation.Ocean:
                rnd = Random.Range(0, oceanObstaclePrefab.Length);
                return oceanObstaclePrefab[rnd];
        }
        return null;
    }
    private void SpawnCoin(float posX, float posY) // Spawns random coin depending on chance
    {
        int rndToSpawn = Random.Range(0, 101);

        if (rndToSpawn < coinChance) // Chance to spawn coin
        {
            GameObject tempCoin = null;

            int chance = Random.Range(0, 101);
            if (chance <= basicCoinChance) // Spawns basic coin
            {
                tempCoin = coinPrefab[0];
            }
            else if (chance <= basicCoinChance + mediumCoinChance) // Spawns medium coin
            {
                tempCoin = coinPrefab[1];
            }
            else if (chance <= basicCoinChance + mediumCoinChance + highCoinChance) // Spawns high coin
            {
                tempCoin = coinPrefab[2];
            }
            Instantiate(tempCoin, new Vector3(posX, posY, 0), Quaternion.Euler(0, 0, 0));
        }
    }
    public static void Shuffle(float[] array) // Shuffles array of positions
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = Random.Range(0, n--);
            float temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }

    public void StopSpawning() // Stops spawning obstacles, when player is dead. Needs to be conected to a player death event
    {
        doSpawn = false;
    }
    public void StartSpawning() // Starts spawning obstacles, when player is dead
    {
        doSpawn = true;
        StartCoroutine(SpawnObstacleCoroutine());
    }
    public void levelChanged(int x)
    {
        currentLocation = (LevelLocation)x;
    }
    public void SetQuantity(float x)
    {
        quantitySpawn = x;
    }
    public void SetOffsetY(float y)
    {
        offsetY = y;
    }
}
