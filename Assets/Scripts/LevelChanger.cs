using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class LevelChanger : MonoBehaviour
{
    // Invokes on level change
    public enum LevelLocation
    {
        Beach = 0,
        Ocean = 1,
        DeepOcean = 2
    };

    [SerializeField] LevelLocation currentLocation;

    public UnityEvent<int> LevelChanged = new UnityEvent<int>(); 
    public UnityEvent TransitionStarted = new UnityEvent(); // Stops spawning when transition starts
    public UnityEvent TransitionStoped = new UnityEvent(); // Starts spawning when transition stops
    
    [Tooltip("Level background and decorative borders. 0 - Beach, 1 - Ocean, 2 - DeepOcean")]
    [SerializeField] GameObject[] levelsLayout;
    [SerializeField] GameObject currentLayout;
    [SerializeField] ObstacleAndCoinSpawnerScript obstacleHandler;
    [Tooltip("How much time needs to pass to change to the next level. 0 - from level 1, 1 - from level 2, 2 - from level 3")]
    [SerializeField] float[] timeToNextLevel;
    [Tooltip("How many seconds does a transition from playing mode, to cutscene takes")]
    [SerializeField] float transitionSpeed;
    [Tooltip("Gow long does a black screen lasts")]
    [SerializeField] float transitionDuration;
    [Tooltip("How many seconds does a cutscene lasts")]
    [SerializeField] float cutsceneLength;
    [Tooltip("Animator that makes screen fade in and fade out")]
    [SerializeField] Animator blackScreen;
    [Tooltip("Animation prefab of cutscene between levels")]
    [SerializeField] GameObject[] cutscenePrefab;
    [Space(7)]
    [SerializeField] SoundSettingsScript soundSettings;
    [SerializeField] AudioClip oceanMusic;


    private GameObject playerGO;
    private bool playerAlive = true;
    
    private void Awake()
    {
        playerGO = GameObject.Find("Player");
    }
    private void Start()
    {
        StartCoroutine(CountDownToNextLevel());
    }
    private IEnumerator CountDownToNextLevel() // Timer to switch to the next level
    {
        float wait = timeToNextLevel[(int)currentLocation];
        yield return new WaitForSeconds(wait); // Goes to a next level after some period of time 

        if (playerAlive)
        {
            StartCoroutine(ChangeLevelCoroutine(currentLocation + 1));
            if ((int)currentLocation < timeToNextLevel.Length - 1)
            {
                StartCoroutine(CountDownToNextLevel());
            }
        }
    }
    IEnumerator ChangeLevelCoroutine(LevelLocation lvl) // To what level it needs to change
    {
        TransitionStarted?.Invoke();

        currentLocation = lvl;
        
        playerGO.GetComponent<PlayerScript>().enabled = false; // Turns off player controller
        playerGO.GetComponent<PolygonCollider2D>().enabled = false; // Turns off colider, so player wont be hit during transition
        blackScreen.SetTrigger("FadeIn"); // Trigger fadeOut animation
        yield return new WaitForSeconds(transitionSpeed); // Waits for a transition to end

        Destroy(currentLayout); // Removes curent layOut of 

        var tempCutscene = Instantiate(cutscenePrefab[(int)currentLocation - 1], new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));

        tempCutscene.transform.Find("PropPlayer").GetComponentInChildren<Animator>().SetTrigger("OceanBeach");
        
        yield return new WaitForSeconds(transitionDuration); // Shows black screen, so every enemy and obstacle would be destroyed
        blackScreen.SetTrigger("FadeOut");
        // Shows cutscene

        yield return new WaitForSeconds(cutsceneLength);
        
        LevelChanged?.Invoke((int)currentLocation);
        blackScreen.SetTrigger("FadeIn"); // Trigger fadeOut animation
        yield return new WaitForSeconds(transitionSpeed);
        soundSettings.ChangeMusic(oceanMusic);
        Destroy(tempCutscene);
        currentLayout = Instantiate(levelsLayout[(int)currentLocation], Vector3.zero, Quaternion.Euler(90, -180, 0)); //Spawns new layout
        playerGO.transform.position = new Vector3(0, -4, 0); // Sets position of player

        blackScreen.SetTrigger("FadeOut");
        yield return new WaitForSeconds(transitionSpeed);

        playerGO.GetComponent<PlayerScript>().enabled = true; // Turns on player controller
        playerGO.GetComponent<PolygonCollider2D>().enabled = true; // Turns on colider, so player wont be hit during transition

        // Changes obstacles spawner stats
        obstacleHandler.SetQuantity(7);
        obstacleHandler.SetOffsetY(0.4f);

        TransitionStoped?.Invoke();
    }
    public void StopCounitng() // Needs to be conected to a player death event
    {
        playerAlive = false;
    }

    public float[] GetLevelLength()
    {
        return timeToNextLevel;
    }
}
