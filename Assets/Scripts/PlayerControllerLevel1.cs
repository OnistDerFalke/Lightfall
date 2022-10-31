using UnityEngine;

public class PlayerControllerLevel1 : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer rend;
    
    public float moveSpeed = 0.1f;
    public float jumpForce = 6f;

    public LayerMask groundLayer;

    private Rigidbody2D rigidBody;
    
    private static readonly int MovementSpeed = Animator.StringToHash("MovementSpeed");
    private static readonly int IsInJump = Animator.StringToHash("IsInJump");
    private float currentSpeed;
    private bool stillOnPlatform;

    private Vector2 startPos;
    
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        startPos = gameObject.transform.position;
    }
    
    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.GS_GAME)
        {
            if (GameManager.instance.IsPlayerDead())
                return;

            currentSpeed = 0;
            FlipSpriteOnMove();

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                MoveRight();
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                MoveLeft();
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                Jump();

            animator.SetFloat(MovementSpeed, currentSpeed);
            animator.SetBool(IsInJump, !IsGrounded());
        }
    }


    #region MOVEMENT METHODS

    private void Jump()
    {
        if (!IsGrounded()) return;
        rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0, 0, Space.World);
        currentSpeed = moveSpeed * Time.deltaTime;
    }
    
    private void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0, Space.World);
        currentSpeed = moveSpeed * Time.deltaTime;
    }

    #endregion
    
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.transform.CompareTag("Platform"))
        {
            stillOnPlatform = true;
        }
    }
    
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.CompareTag("Platform"))
        {
            stillOnPlatform = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Battery"))
        {
            GameManager.instance.AddBattery();
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("Portal"))
        {
            if (GameManager.instance.AreAllHoesTaken())
            {
                GameManager.instance.LevelCompleted();
            }
        }
        
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<EnemyController>().onceKilled) return;
            if (transform.position.y <=
                other.transform.position.y + other.GetComponent<EnemyController>().jumpHeightToKill)
            {
                KillPlayer();
            }
            else
            {
                GameManager.instance.AddDefeatedEnemy();
                other.GetComponent<EnemyController>().KillEnemy();
            }
        }

        if (other.CompareTag("BlueHue"))
        {
            other.gameObject.SetActive(false);
            GameManager.instance.AddHue(Hue.BLUE);
        }
        
        if (other.CompareTag("RedHue"))
        {
            other.gameObject.SetActive(false);
            GameManager.instance.AddHue(Hue.RED);
        }
        
        if (other.CompareTag("GreenHue"))
        {
            other.gameObject.SetActive(false);
            GameManager.instance.AddHue(Hue.GREEN);
        }

        if (other.CompareTag("Live"))
        {
            GameManager.instance.AddLive();
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("DeathZone"))
        {
            KillPlayer();
        }
    }
    
    private void FlipSpriteOnMove()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            rend.flipX = true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            rend.flipX = false;
        }
    }

    private bool IsGrounded()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer.value);
        if (stillOnPlatform) 
            return true;

        return hit.collider != null;
    }

    public void KillPlayer()
    {
        if(GameManager.instance.GetLives() > 0)
        {
            gameObject.transform.position = startPos;
            if(LevelGenerator.instance != null)
                LevelGenerator.instance.RestartGenerator();
            rend.flipX = true;
            GameManager.instance.ResetLightPower();
            GameManager.instance.RemoveLive();
        }
    }
}
