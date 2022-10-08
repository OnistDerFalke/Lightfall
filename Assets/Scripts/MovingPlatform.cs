using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float distance;
    public float moveSpeed;
    public bool isVertical;

    private float root;
    private int dir;
        
    void Awake()
    {
        if(isVertical) root = transform.position.y;
        else root = transform.position.x;
    }

    void Update()
    {
       HandleMovement();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
            other.collider.transform.SetParent(transform);
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
            other.collider.transform.SetParent(null);
    }

    private void HandleMovement()
    {
        if(isVertical)
        {
            if(transform.position.y > root+distance) 
            {
                if(dir == 1) 
                    distance *= -1;
                dir = -1;
            }
            else if(transform.position.y < root+distance) 
            {
                if(dir == -1) 
                    distance *= -1;
                dir = 1;
            }
            transform.Translate(0, dir * moveSpeed * Time.deltaTime, 0, Space.World);
        }
        else
        {
            if(transform.position.x > root+distance) 
            {
                if(dir == 1) 
                    distance *= -1;
                dir = -1;
            }
            else if(transform.position.x < root+distance) 
            {
                if(dir == -1) 
                    distance *= -1;
                dir = 1;
            }
            transform.Translate(dir * moveSpeed * Time.deltaTime, 0, 0, Space.World);
        }
    }
}
