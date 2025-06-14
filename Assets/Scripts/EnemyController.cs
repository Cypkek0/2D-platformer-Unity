using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyController : MonoBehaviour
{
    private bool isFacingRight = false;
    public Animator animator;
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 0.1f; // Prêdkoœæ ruchu gracza
    private float startPositionX;
    [Range(0.01f, 20.0f)][SerializeField] private float moveRange = 1.0f;
    private bool isMovingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        startPositionX = this.transform.position.x;
        // Pobranie komponentu Rigidbody2D z awatara
       // rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("isDead", false);
    }
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x = -theScale.x;
        transform.localScale = theScale;
    }
    // Update is called once per frame
    void Update()
    {
        if(isMovingRight)
        {
            if(this.transform.position.x < startPositionX+moveRange) 
            {
                MoveRight();
            } else
            {
                
                isMovingRight = false;
            }
        } 
        else
        {
            if(this.transform.position.x > startPositionX-moveRange) 
            {
                MoveLeft();
                
            }
            else
            {
                isMovingRight = true;
            }

        }
    }
    void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        if (!isFacingRight)
        {
            Flip();
        }
    }
    void MoveLeft()
    {
        transform.Translate(moveSpeed * Time.deltaTime * -1, 0.0f, 0.0f, Space.World);
        if (isFacingRight)
        {
            Flip();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
 

        if (col.CompareTag("Player"))
        {
           if( col.gameObject.transform.position.y > this.transform.position.y)
            {
                animator.SetBool("isDead", true);
                Die();
                

            }
        }


    }
    void Die()
    {
        StartCoroutine(KillOnAnimationEnd());

    }
    IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);

    }

}
