using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControllers : MonoBehaviour
{
    [Header("Movement parameters")] // Nag³ówek grupuj¹cy parametry ruchu
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 0.1f; // Prêdkoœæ ruchu gracza
    [SerializeField] private float jumpForce = 1.0f; // Wysokoœæ skoku
    [SerializeField] private float bounceForce = 50.0f;
    [SerializeField] private float jumpBufferTime = 0.05f; // Wysokoœæ skoku
    private float jumpBufferCounter = 0f;
    [SerializeField]  private float releaseDelay = 0.1f; // OpóŸnienie przed sprawdzeniem, czy przycisk nie jest wciœniêty
    private float releaseDelayTimer = 0f; // Licznik czasu dla opóŸnienia
    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimeTimer = 0f;
    private float horizontalInput = 0.0f;
    private bool shouldBounce = false;



    [Header("Audio Clips")]
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip hpPickupSound;
    [SerializeField] private AudioClip keyPickupSound;
    [SerializeField] private AudioClip enemyKilledSound;
    [SerializeField] private AudioClip levelFinishedSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip jumpSound;
    
    private AudioSource source;

    [Header("Jump system")]
    [SerializeField] float fallMultiplier;
    [SerializeField] float jumpTime;
    [SerializeField] float jumpMultiplier;
    bool isJumping;
    float jumpCounter;
    float gravityMax = 5.5f;
    Vector2 vecGravity;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public LayerMask ladderLayer;
    [Space(10)] // Dodatkowy odstêp w inspektorze

    public Animator animator;
    public float rayLengthGround = 1.1f;
    public float rayLengthLadder = 1.1f;
    private bool isWalking = false;
    private bool isFacingRight = true;
    private bool isClimbing = false;
    private float vertical = 0f;
    private int keysFound = 0;
    private int keysNumber = 3;
    private Vector3 startPosition;
    

    private Rigidbody2D rigidBody; // Pole na komponent Rigidbody2D
    float gravityScale = 2.5f; 
    bool IsGrounded()
    {

        Vector2 left = new Vector2(this.transform.position.x - 0.45f, this.transform.position.y);  // Lewy bok
        Vector2 right = new Vector2(this.transform.position.x + 0.45f, this.transform.position.y); // Prawy bok


        bool leftHit = Physics2D.Raycast(left, Vector2.down, rayLengthGround, groundLayer.value);  // Promieñ z lewej strony
        bool rightHit = Physics2D.Raycast(right, Vector2.down, rayLengthGround, groundLayer.value); // Promieñ z prawej strony


        return leftHit || rightHit; 
    }

    bool IsLadder()
    {
        Vector2 middle = new Vector2(this.transform.position.x, this.transform.position.y);  // srodek
        bool middleRaycast = Physics2D.Raycast(middle, Vector2.down, rayLengthLadder, ladderLayer.value);
        return middleRaycast;
        
    }
    void DamageTaken()
    {
        if (GameManager.instance.GetLives() > 0)
        {
            source.PlayOneShot(hitSound, AudioListener.volume);
            GameManager.instance.ManageHealth(-1);
            transform.position = startPosition;
        }
        else if (GameManager.instance.GetLives() == 0)
        {
            Debug.Log("game over");
            Die();
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("FallLevel"))
        {
            DamageTaken();
        }
        if(col.CompareTag("Sign"))
        {
            startPosition = transform.position;
        }
        if(col.CompareTag("Spike"))
        {
           DamageTaken();
        }
        if (col.CompareTag("Enemy"))
        {
            if (col.gameObject.transform.position.y < this.transform.position.y)
            {
                GameManager.instance.AddPoints(1000);
                source.PlayOneShot(enemyKilledSound, AudioListener.volume);
                GameManager.instance.EnemyKilled();
             
            }
            else if (col.gameObject.transform.position.y > this.transform.position.y)
            {

                DamageTaken();
                
            }
        }
        if (col.CompareTag("Kustosz"))
        {
            DamageTaken();
        }

        if (col.gameObject.CompareTag("FallLevel")) Debug.Log("SPADLES");

        if (col.gameObject.CompareTag("Gem"))
        {
            keysFound++;
            SpriteRenderer spriteRenderer = col.gameObject.GetComponent<SpriteRenderer>();
            Color gemColor = spriteRenderer.color;
            GameManager.instance.AddKeys(gemColor);
            source.PlayOneShot(keyPickupSound, AudioListener.volume);
            col.gameObject.SetActive(false);
        }

        if (col.gameObject.CompareTag("MovingPlatform"))
        {
            transform.SetParent(col.transform);
        }

        if (col.gameObject.CompareTag("Finish"))
        {
           if(keysFound == keysNumber)
            {
            
                source.PlayOneShot(levelFinishedSound, AudioListener.volume);
                GameManager.instance.AddPoints(GameManager.instance.GetLives()*1000);
                GameManager.instance.LevelCompleted();
                
            }
           else
            {
                Die();
            }
        }

        if (col.gameObject.CompareTag("Finish2"))
        {
            if (keysFound == keysNumber)
            {

                source.PlayOneShot(levelFinishedSound, AudioListener.volume);
                GameManager.instance.AddPoints(GameManager.instance.GetLives() * 1000);
                GameManager.instance.LevelCompleted();

            }
            else
            {
                Die();
            }
        }

        if (col.gameObject.CompareTag("Heart"))
        {
            source.PlayOneShot(hpPickupSound, AudioListener.volume);
            GameManager.instance.ManageHealth(1); 
            col.gameObject.SetActive(false);
        }

        if (col.CompareTag("Bonus"))
        {
            GameManager.instance.AddPoints(1000);
            source.PlayOneShot(coinSound, AudioListener.volume);
            col.gameObject.SetActive(false);
        }

    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Ladder"))
        {
            vertical = Input.GetAxis("Vertical");
            
        }
    }


    void OnTriggerExit2D(Collider2D col)
    {

        if(col.gameObject.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null);
        }
        //Debug.Log(col.gameObject.CompareTag("Ladder"));
        //Debug.Log("aaaa");
        if (!col.gameObject.CompareTag("Ladder")) return;
        rigidBody.gravityScale = gravityScale;
        isClimbing = false;


    }

    


    // Metoda wywo³ywana, gdy obiekt jest inicjalizowany
    void Awake()
    {
        source = GetComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
    }
    void Start()
    {
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        rigidBody.gravityScale = gravityScale;
    }
    
    // Update is called once per frame
    void Update()
    {

        if (GameManager.instance.currentGameState == GameState.GAME)
        {

            isWalking = false;
            horizontalInput = 0.0f;

            // Ruch w prawo
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                //transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                horizontalInput = 1.0f; 
                isWalking = true;
                if (!isFacingRight)
                {
                    Flip();
                }

            }

            // Ruch w lewo
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                //transform.Translate(moveSpeed * Time.deltaTime * -1, 0.0f, 0.0f, Space.World);
                horizontalInput = -1.0f;
                isWalking = true;
                if (isFacingRight)
                {
                    Flip();
                }
            }

            HandleJump();



            animator.SetBool("isGrounded", IsGrounded());
            animator.SetBool("isWalking", isWalking);
            animator.SetBool("isClimbing", isClimbing);

        }



    }
    void FixedUpdate()
    {
      

        if (GameManager.instance.currentGameState == GameState.GAME)
        {
            rigidBody.velocity = new Vector2(horizontalInput * moveSpeed, rigidBody.velocity.y);
        }

        if (isJumping)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce); 
            isJumping = false; 
        }

        if (rigidBody.velocity.y > 0 && jumpCounter < jumpTime)
        {
            
            jumpCounter += Time.fixedDeltaTime * jumpForce;
            float t = jumpCounter / jumpTime;
            float currentJumpM = jumpMultiplier;

            if (t > 0.2f)
            {
                currentJumpM = jumpMultiplier * (1 - t);
            }

            rigidBody.velocity += vecGravity * currentJumpM * Time.fixedDeltaTime;
        }

        if (releaseDelayTimer > 0)
        {
            
            releaseDelayTimer -= Time.fixedDeltaTime;
        }
        else if (!Input.GetButton("Jump") && rigidBody.velocity.y > 0)
        {
            
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.6f);
        }

        if (rigidBody.velocity.y < 0)
        {
            
            if (rigidBody.velocity.y > -gravityMax)
            {
                rigidBody.velocity -= vecGravity * fallMultiplier * Time.fixedDeltaTime;
            }
        }

        if (IsLadder())
        {

            if (vertical >0)
            {
                isClimbing = true;
            }

            if (isClimbing)
            {
                rigidBody.gravityScale = 0;
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, vertical * moveSpeed);
            } 
            
        }
        else
        {
            rigidBody.gravityScale = gravityScale;
            isClimbing = false;
        }

    }

    void HandleJump()
    {
        if(isClimbing)
        {
            return; //bez tego drabina nie dziala; wylacza to skok podczas wspinania wiec chyba git
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (IsGrounded())
        {
            coyoteTimeTimer = coyoteTime; 
        }
        else
        {
            coyoteTimeTimer -= Time.deltaTime; 
        }

        if ((Input.GetKeyDown(KeyCode.Space)|| jumpBufferCounter >0) && coyoteTimeTimer >0)
        {
            
            
            isJumping = true;
            jumpCounter = 0;
            releaseDelayTimer = releaseDelay;
            source.PlayOneShot(jumpSound, AudioListener.volume);
            jumpBufferCounter = 0;
            coyoteTimeTimer = 0;
        }

    }

    void Flip()
    {
        isFacingRight = !isFacingRight; 
        Vector3 theScale = transform.localScale;
        theScale.x = -theScale.x;
        transform.localScale = theScale;
    }

    void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        Destroy(gameObject);
        GameManager.instance.SetGameState(GameState.GAMEOVER);
    }
    
}
