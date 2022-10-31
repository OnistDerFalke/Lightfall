using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public float distance;
    public float moveSpeed;
    public float jumpHeightToKill;
    public bool onceKilled;

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer rend;
    
    private float rootX;
    private int dir;
        
    void Awake()
    {
        rootX = transform.localPosition.x;
    }

    void Update()
    {
        HandleFlip();
        HandleMovement();
    }

    public void KillEnemy()
    {
        StartCoroutine(KillAfterDeath());
        onceKilled = true;
    }

    private void HandleMovement()
    {
        if(transform.localPosition.x > rootX+distance) 
        {
            if(dir == 1) 
                distance *= -1;
            dir = -1;
        }
        else if(transform.localPosition.x < rootX+distance) 
        {
            if(dir == -1) 
                distance *= -1;
            dir = 1;
        }
    
        transform.Translate(dir * moveSpeed * Time.deltaTime, 0, 0, Space.Self);
    }

    private void HandleFlip()
    {
        if(dir == 1) 
            rend.flipX = true;
        else if(dir == -1) 
            rend.flipX = false;
    }

    private IEnumerator KillAfterDeath()
    {
        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        Debug.Log("Enemy is killed!");
    }
}
