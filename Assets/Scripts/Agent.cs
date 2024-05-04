using UnityEngine;
using Random = System.Random;

public class Agent : MonoBehaviour
{
    [SerializeField] public float speed = 5f;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Random _random = new Random();
    [SerializeField] DragDrop dragDrop;
    [SerializeField] Player player;
    [SerializeField] Animator playerAnimator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //movement = Vector2.right;
        movement = new Vector2(1, 0);
    }

    private void FixedUpdate()
    {
        //rb.velocity = movement * speed * Time.fixedDeltaTime;
        transform.Translate(movement * speed * Time.fixedDeltaTime);
        FlipSprite();
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
            //TODO: Display Game Over Screen
            speed = 0;
            dragDrop.allowDrag = false;
            player.stopDecrease = true;
            playerAnimator.SetBool("run", false);
            
        }
    }


    private void FlipSprite()
    {
        if (movement.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (movement.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }
}