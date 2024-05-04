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
    [SerializeField] public int lives = 3;
    [SerializeField] private GameObject heart1, heart2, heart3;
    [SerializeField] private AudioClip liveDecreaseSFX;
    
    private Vector2 movement;
    private Rigidbody2D rb;
    private Random _random = new Random();
    private bool amICovered = false;
    private float initialSpeed;
    
    public bool gameOver = false;
    public bool gameStarted = false;
    
    private float nextActionTime = 0.0f;
    public float periodToWait = 0.65f;
    
    
    void Start()
    {
        initialSpeed = speed;
        rb = GetComponent<Rigidbody2D>();
        //movement = Vector2.right;
        movement = new Vector2(0, 0);
        playerAnimator.SetBool("idle", true);
    }
    
    private void Update () {
        if(amICovered) Debug.Log("Agent is covered");

        if (!gameOver && gameStarted)
        {
            if (Time.time > nextActionTime ) { //Every 1 second, check if the agent is covered and if so, wait for a random time between 1 and 4 seconds
                nextActionTime += periodToWait;
                if (amICovered)
                {
                    Debug.Log("Agent is covered from Update()");
                    int randomValue = _random.Next(0, 100);
            
                    if(randomValue < 20)
                        StartCoroutine(WaitUnderCover());
                }
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        lives--;
        
        if (lives == 2)
        {
            heart3.SetActive(false);
            //AudioSource.PlayClipAtPoint(liveDecreaseSFX, transform.position);
            //new Vector3(0,0,Camera.main.transform.position.z
            AudioSource.PlayClipAtPoint(liveDecreaseSFX,  new Vector3(0,0,Camera.main.transform.position.z));
        }
        else if (lives == 1)
        {
            heart2.SetActive(false);
            //AudioSource.PlayClipAtPoint(liveDecreaseSFX, transform.position);
            AudioSource.PlayClipAtPoint(liveDecreaseSFX,  new Vector3(0,0,Camera.main.transform.position.z));
        }
        else if (lives == 0)
        {
            heart1.SetActive(false);
            //AudioSource.PlayClipAtPoint(liveDecreaseSFX, transform.position);
            AudioSource.PlayClipAtPoint(liveDecreaseSFX,  new Vector3(0,0,Camera.main.transform.position.z));
        }
        
        if (lives <= 0)
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
    }

    IEnumerator WaitUnderCover()
    {
        speed = 0;
        audioSource.Pause();
        playerAnimator.SetBool("idle", true);
        
        //Wait between 1 and 3 seconds including float values
        float randomSeconds = _random.Next(1, 3) + (float)_random.NextDouble();
        yield return new WaitForSeconds(randomSeconds);
        
        playerAnimator.SetBool("idle", false);
        speed = initialSpeed;
        audioSource.UnPause();
    }
    
    IEnumerator WaitBeforeDisplayingWinPanel()
    {
        yield return new WaitForSeconds(1.5f);
        if(!losePanel.activeSelf)
            winPanel.SetActive(true);
    }

    private void FixedUpdate()
    {
        if(!gameOver && gameStarted)
            transform.Translate(movement * speed * Time.fixedDeltaTime);
        FlipSprite();

        if (!gameOver && gameStarted)
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
    
    public void StartAgentMovement()
    {
        movement = Vector2.right;
        gameStarted = true;
        playerAnimator.SetBool("idle", false);
        playerAnimator.SetBool("run", true);
    }
}