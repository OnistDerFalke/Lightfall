using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControllerLevel1 : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    public float jumpForce = 6f;

    private Rigidbody2D rigidBody;
    
    void Start()
    {
        
    }

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0, Space.World);
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Translate(-moveSpeed * Time.deltaTime, 0, 0, Space.World);

        if (Input.GetMouseButtonDown(0))
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
