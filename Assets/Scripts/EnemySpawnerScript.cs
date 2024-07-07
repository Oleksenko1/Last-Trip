using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    public enum LevelLocation
    {
        Beach = 0,
        Ocean = 1,
        DeepOcean = 2
    };
    [Header("Enemies")]
    [SerializeField] LevelLocation currentLocation;
    [Tooltip("1 - Eagle, 2 - Lizard, 3 - ?Crocodile?")]
    [SerializeField] GameObject[] beachEnemies;
    [SerializeField] GameObject[] oceanEnemies;
    [Tooltip("1 - Eagle X coordinate, 2 - Lizard Y coordinate, 3 - Crocodile X coordinate")]
    [SerializeField] float[] enemyBorderPosition;
    [Tooltip("How far away from player direction - can enemy spawn. 1 - eagle Y, 2 - lizard X, 3 - crocodile Y")]
    [SerializeField] float[] offsetOfPosition;
    [Tooltip("Borders of how far away from center can enemies spawn. 1 - eagle Y, 2 - lizard X, 3 - crocodile Y")]
    [SerializeField] float[] borderOfOffset;
    //[Tooltip("1 - Eagle, 2 - Lizard, 3 - Crocodile")]
    //[SerializeField] Vector2[] spawnPoints;

    [Header("Danger signs")]
    [SerializeField] GameObject dangerSign;
    [SerializeField] GameObject dangerZone;
    [SerializeField] float dangerSignBorderX;
    [SerializeField] float dangerSignBorderY;
    [SerializeField] float minCautionTime;
    [SerializeField] float flexibleCautionTime;

    [Header("Spawn characteristics")]
    [SerializeField] float quantity;
    [SerializeField] int chanceToSpawn;
    [Tooltip("How much time to wait to spawn a new enemy")]
    [SerializeField] int coolDown;

    private int delay;
    bool doSpawn = true;
    private DifficultyMultiplierScript difficultyMultiplier;

    private Transform playerPos;

    private int enemyIndex;
    private Quaternion enemyRotation;
    private Vector3 enemyPosition = Vector2.zero;
    private GameObject enemyToSpawn;
    private void Awake()
    {
        difficultyMultiplier = GameObject.Find("DifficultyMultiplier").GetComponent<DifficultyMultiplierScript>();
        playerPos = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void Start()
    {
        StartCoroutine(EnemySpawnCoroutine());
    }

    IEnumerator EnemySpawnCoroutine()
    {
        while (doSpawn)
        {
            delay = 0; // Resets cooldown on a new itteration
            int rnd = Random.Range(0, 101);
            if (rnd < chanceToSpawn)
            {
                GetRandomEnemy();
                GetEnemySpawnPosition();

                float overallSpawnDelay = minCautionTime + flexibleCautionTime / difficultyMultiplier.GetMultiplier();
                StartCoroutine(SpawnEnemyAfterDealy(enemyToSpawn, enemyPosition, overallSpawnDelay));

                SpawnCautionSigns(overallSpawnDelay);
            }
            yield return new WaitForSeconds(20 / (quantity * difficultyMultiplier.GetMultiplier()) + delay);
        }
    }
    
    private void GetRandomEnemy() // Returns random enemy and sets his position, index. 1
    {
        int rnd;
        switch (currentLocation)
        {
            case LevelLocation.Beach:
                rnd = Random.Range(0, beachEnemies.Length);
                delay += coolDown;
                enemyIndex = rnd;
                enemyToSpawn = beachEnemies[rnd];
                break;

            case LevelLocation.Ocean:
                rnd = Random.Range(0, oceanEnemies.Length);
                delay += coolDown;
                enemyIndex = rnd;
                enemyToSpawn = oceanEnemies[rnd];
                break;

            case LevelLocation.DeepOcean:
                break;
        }
    }
    private void GetEnemySpawnPosition() // Sets position of spawned enemy. 2
    {
        switch (enemyIndex)
        {
            case 0: // For eagle
                int rnd = Random.Range(0, 2); // Choose random position 
                int[] leftOrRight = { 1, -1 };

                // Position on Y coordinates of eagle. Randomises offset
                float yPosEagle = playerPos.position.y + Random.Range(-offsetOfPosition[enemyIndex], offsetOfPosition[enemyIndex]);
                yPosEagle = Mathf.Clamp(yPosEagle, -borderOfOffset[enemyIndex], borderOfOffset[enemyIndex]);
                enemyPosition = new Vector3(enemyBorderPosition[enemyIndex] * leftOrRight[rnd], yPosEagle, 0);
                
                //Rotates eagle in needed direction
                enemyToSpawn.transform.rotation = Quaternion.Euler(0, 0, -90 * leftOrRight[rnd]);
                enemyRotation = Quaternion.Euler(0, 0, 90 * leftOrRight[rnd]);
                
                break;
            case 1: // For lizard

                // Position on X coordinates of lizard. Randomises offset
                float xPosLizard = playerPos.position.x + Random.Range(-offsetOfPosition[enemyIndex], offsetOfPosition[enemyIndex]);
                xPosLizard = Mathf.Clamp(xPosLizard, -borderOfOffset[enemyIndex], borderOfOffset[enemyIndex]);
                enemyPosition = new Vector3(xPosLizard, enemyBorderPosition[enemyIndex], 0);
                
                enemyRotation = enemyToSpawn.transform.rotation;
                
                break;
        }
    }
    private IEnumerator SpawnEnemyAfterDealy(GameObject GO, Vector3 vecPos, float afterTime) // Spawns enemy. 3
    {
        yield return new WaitForSeconds(afterTime);
        Instantiate(GO, vecPos, GO.transform.rotation);
    }
    private void SpawnCautionSigns(float destroyAfterSeconds) // Creates caution signs. 4
    {
        Vector3 signPos;
        // Clamps position of 
        signPos = new Vector3(Mathf.Clamp(enemyPosition.x, -dangerSignBorderX, dangerSignBorderX), Mathf.Clamp(enemyPosition.y, -dangerSignBorderY, dangerSignBorderY), 0);

        var sign = Instantiate(dangerSign, signPos, dangerSign.transform.rotation);
        var zone = Instantiate(dangerZone, signPos, dangerZone.transform.rotation);

        zone.transform.rotation = enemyRotation;
        Destroy(sign, destroyAfterSeconds);
        Destroy(zone, destroyAfterSeconds);
    }
    public void StopSpawning() // Stops spawning enemies, when player is dead. Needs to be conected to a player death event
    {
        doSpawn = false;
    }
    public void StartSpawning() // Starts spawning enemies, when player is dead
    {
        doSpawn = true;
        StartCoroutine(EnemySpawnCoroutine());
    }
    public void levelChanged(int x) 
    {
        currentLocation = (LevelLocation)x;
    }

}
