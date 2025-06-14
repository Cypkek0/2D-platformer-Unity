using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class BearController : MonoBehaviour
{
    private bool isFacingRight = true;
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
       
        animator = GetComponent<Animator>();
        
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
 



    }
    void Die()
    {
        

    }
   

}
