using UnityEngine;

public class PlayerControllerLevel2 : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer rend;
    
    public float moveSpeed = 0.1f;
    public float jumpForce = 6f;
    public float moveSpeedStep;
    public float jumpForceStep;
    public Vector2 moveSpeedRange;
    public Vector2 jumpForceRange;

    public LayerMask groundLayer;

    private Rigidbody2D rigidBody;
    
    private static readonly int MovementSpeed = Animator.StringToHash("MovementSpeed");
    private static readonly int IsInJump = Animator.StringToHash("IsInJump");
    private float currentSpeed;
    private bool stillOnPlatform;
    private float baseMoveSpeed;
    private float baseJumpForce;

    private Vector2 startPos;
    
    void Awake()
    {
        baseMoveSpeed = moveSpeed;
        baseJumpForce = jumpForce;
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
            
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                Jump();
            
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                if (moveSpeed - moveSpeedStep >= moveSpeedRange.x)
                    moveSpeed -= moveSpeedStep;
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                if (moveSpeed + moveSpeedStep <= moveSpeedRange.y)
                    moveSpeed += moveSpeedStep;
            
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
                if (jumpForce - jumpForceStep >= jumpForceRange.x)
                   jumpForce -= jumpForceStep;
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                if (jumpForce + jumpForceStep <= jumpForceRange.y)
                    jumpForce += jumpForceStep;
            
            MoveRight();
            animator.SetFloat(MovementSpeed, currentSpeed);
            animator.SetBool(IsInJump, !IsGrounded());
            Debug.Log(animator.GetFloat(MovementSpeed));
        }
    }


    #region MOVEMENT METHODS

    private void Jump()
    {
        if (!IsGrounded()) return;
        rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
                RemoveLife();
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

    private bool IsGrounded()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer.value);
        if (stillOnPlatform) 
            return true;

        return hit.collider != null;
    }

    private void KillPlayer()
    {
        if(GameManager.instance.GetLives() > 0)
        {
            gameObject.transform.position = startPos;
            if(LevelGenerator.instance != null)
                LevelGenerator.instance.RestartGenerator();
            rend.flipX = true;
            GameManager.instance.ResetLightPower();
            GameManager.instance.RemoveLive();
            moveSpeed = baseMoveSpeed;
            jumpForce = baseJumpForce;
        }
    }
    
    private void RemoveLife()
    {
        if(GameManager.instance.GetLives() > 0)
            GameManager.instance.RemoveLive();
    }
}
