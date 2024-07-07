using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [Tooltip("How much whould an animation of player depend on his movement direction")]
    [Range(0, 1)]
    [SerializeField] float animSlowingMult;
    [SerializeField] public UnityEvent playerDied = new UnityEvent(); // Event for player death
    [SerializeField] public UnityEvent<int> coinCollected = new UnityEvent<int>(); // On picking up coin 
    [SerializeField] SoundSettingsScript soundSettings;
    bool isAlive = true;
    private Vector2 moveVector;
    private Rigidbody2D rb;
    private DifficultyMultiplierScript difficultyMultiplier;
    private Animator anim;
    private void Awake()
    {
        difficultyMultiplier = GameObject.Find("DifficultyMultiplier").GetComponent<DifficultyMultiplierScript>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }
    public void SetMoveVector(Vector2 vec2)
    {
        moveVector = vec2;
    }

    public void Update()
    {
        if(isAlive) // Moving player, when he is alive
        {
            rb.velocity = moveVector * moveSpeed;
            anim.SetFloat("RunSpeed", 1.3f + moveVector.y * animSlowingMult);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Player collided with colider");
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            playerIsDead();

            soundSettings.PlayPLayerColided();
            difficultyMultiplier.SetMultiplier(0);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin")) // On picking up coin
        {
            coinCollected?.Invoke(collision.GetComponent<CoinScript>().Collect());
        }
        else if (collision.gameObject.CompareTag("EagleEnemy") || collision.gameObject.CompareTag("LizardEnemy"))
        {
            playerIsDead();

            soundSettings.PlayPlayerStolen();

            gameObject.transform.SetParent(collision.gameObject.transform); // Set player as child of enemy

            gameObject.GetComponent<PolygonCollider2D>().enabled = false; // Turns off player colider
            // Something else
        }
    }
    private void OnDisable() // So player wont fly away during level transition
    {
        rb.velocity = Vector2.zero;
    }
    private void playerIsDead()
    {
        Debug.Log("GameOver");
        playerDied?.Invoke();
        isAlive = false;
        rb.velocity = Vector2.zero;
        anim.SetFloat("RunSpeed", 0);
    }
}
