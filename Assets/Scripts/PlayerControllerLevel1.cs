using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    
    private int score;
    private int crystalsNumber, maxCrystalsNumber;
    private int lives;

    private Vector2 startPos;
    
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        startPos = gameObject.transform.position;
        score = 0;
        crystalsNumber = 0;
        maxCrystalsNumber = 3;
        lives = 3;
    }
    
    void Update()
    {
        Debug.Log(lives + " " + crystalsNumber);
        currentSpeed = 0;
        FlipSpriteOnMove();
        
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) MoveRight();
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) MoveLeft();
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) Jump();

        animator.SetFloat(MovementSpeed, currentSpeed);
        animator.SetBool(IsInJump, !IsGrounded());
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
            score += 1;
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("Portal"))
        {
            if(crystalsNumber >= maxCrystalsNumber)
                Debug.Log("Level passed!");
            else Debug.Log("You have not enough crystals to pass!");
        }
        
        if (other.CompareTag("Enemy"))
        {
            if(transform.position.y < other.transform.position.y + other.GetComponent<EnemyController>().jumpHeightToKill)
                KillPlayer();
        }

        if (other.CompareTag("Crystal"))
        {
            crystalsNumber++;
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("Live"))
        {
            lives++;
            other.gameObject.SetActive(false);
        }
    }
    
    private void FlipSpriteOnMove()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            rend.flipX = true;
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            rend.flipX = false;
    }

    private bool IsGrounded()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer.value);
        if (stillOnPlatform) return true;
        return hit.collider != null;
    }

    private void KillPlayer()
    {
        if(lives>0)
        {
            gameObject.transform.position = startPos;
            lives--;
        }
        else
        {

        }
    }
}
