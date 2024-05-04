using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class Agent : MonoBehaviour
{
    [SerializeField] public float speed = 5f;
    [FormerlySerializedAs("dragDrop")] [SerializeField] Cloud cloud;
    [SerializeField] Player player;
    [SerializeField] Animator playerAnimator;
    [SerializeField] private GameObject winPanel, losePanel;
    [SerializeField] private AudioSource audioSource;
    
    private Vector2 movement;
    private Rigidbody2D rb;
    private Random _random = new Random();
    private bool amICovered = false;
    private float initialSpeed;
    
    public bool gameOver = false;
    
    private float nextActionTime = 0.0f;
    public float periodToWait = 0.65f;
    
    
    void Start()
    {
        initialSpeed = speed;
        rb = GetComponent<Rigidbody2D>();
        //movement = Vector2.right;
        movement = new Vector2(1, 0);
    }
    
    private void Update () {
        if(amICovered) Debug.Log("Agent is covered");

        if (!gameOver)
        {
            if (Time.time > nextActionTime ) { //Every 1 second, check if the agent is covered and if so, wait for a random time between 1 and 4 seconds
                nextActionTime += periodToWait;
                if (amICovered)
                {
                    int randomValue = _random.Next(0, 100);
            
                    if(randomValue < 20)
                        StartCoroutine(WaitUnderCover());
                }
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        gameOver = true;
        speed = 0;
        audioSource.Pause();
        cloud.allowDrag = false;
        player.stopDecrease = true;
        playerAnimator.SetBool("die", true);
        player.StopRain();
        StartCoroutine(WaitBeforeDisplayingWinPanel());
    }

    IEnumerator WaitUnderCover()
    {
        speed = 0;
        audioSource.Pause();
        playerAnimator.SetBool("idle", true);
        
        //Wait between 1 and 3 seconds including float values
        float randomSeconds = _random.Next(1, 3) + (float)_random.NextDouble();
        Debug.Log("Waiting for " + randomSeconds + " seconds");
        yield return new WaitForSeconds(randomSeconds);
        
        playerAnimator.SetBool("idle", false);
        speed = initialSpeed;
        audioSource.UnPause();
    }
    
    IEnumerator WaitBeforeDisplayingWinPanel()
    {
        yield return new WaitForSeconds(1.5f);
        winPanel.SetActive(true);
    }

    private void FixedUpdate()
    {
        if(!gameOver)
            transform.Translate(movement * speed * Time.fixedDeltaTime);
        FlipSprite();

        if (!gameOver)
        {
            //+0.75f to make the center of the ray a bit higher
            Vector3 leftShoulder = new Vector3(transform.position.x - 0.3f, transform.position.y + 0.75f, transform.position.z);
            Vector3 rightShoulder = new Vector3(transform.position.x + 0.3f, transform.position.y + 0.75f, transform.position.z);
        
            RaycastHit2D hitFromLeftShoulderRay = Physics2D.Raycast(leftShoulder, Vector2.up, 5);
            Debug.DrawRay(leftShoulder, Vector2.up * 5, Color.red);
            
            RaycastHit2D hitFromRightShoulderRay = Physics2D.Raycast(rightShoulder, Vector2.up, 5);
            Debug.DrawRay(rightShoulder, Vector2.up * 5, Color.red);

            try
            {
                if (hitFromLeftShoulderRay.collider.CompareTag("Cover") && hitFromRightShoulderRay.collider.CompareTag("Cover")) // To prevent waiting at the edges of the covers
                {
                    amICovered = true;
                }
                else
                {
                    amICovered = false;
                }
            }
            catch (Exception e) {}
        }
        
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("AgentTriggerer"))
        {
            int randomValue = _random.Next(0, 100);
            
            if(randomValue < 65)
                movement = Vector2.right;
            else if(randomValue >= 65)
                movement = Vector2.left;
            
        }
        else if (other.gameObject.CompareTag("LeftBlockerTriggerer"))
        {
            movement = Vector2.right;
        }
        else if (other.gameObject.CompareTag("EndTriggerer"))
        {
            gameOver = true;
            StartCoroutine(WaitBeforeDisplayingLosePanel());
            speed = 0;
            audioSource.Pause();
            //dragDrop.allowDrag = false;
            player.stopDecrease = true;
            playerAnimator.SetBool("idle", true);
        }
    }
    
    IEnumerator WaitBeforeDisplayingLosePanel()
    {
        yield return new WaitForSeconds(1.5f);
        losePanel.SetActive(true);
    }

    private void FlipSprite()
    {
        if (movement.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (movement.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }
}