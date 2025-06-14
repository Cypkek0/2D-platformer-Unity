using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController2 : MonoBehaviour
{
    private bool isFacingRight = true;
    public Animator animator;
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 0.1f; // Prêdkoœæ ruchu gracza
    private float startPositionX;
    [Range(0.01f, 20.0f)][SerializeField] private float moveRange = 1.0f;
    private bool isMovingRight = true;

    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private Transform rockSpawnPoint;
    [Range(0.1f, 10.0f)][SerializeField] private float rockDropInterval = 2.0f;
    
    private float rockDropTimer;
    void Awake()
    {
        startPositionX = this.transform.position.x;
        rockDropTimer = rockDropInterval;
        animator = GetComponent<Animator>();

    }
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x = -theScale.x;
        transform.localScale = theScale;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingRight)
        {
            if (this.transform.position.x < startPositionX + moveRange)
            {
                MoveRight();
            }
            else
            {

                isMovingRight = false;
            }
        }
        else
        {
            if (this.transform.position.x > startPositionX - moveRange)
            {
                MoveLeft();

            }
            else
            {
                isMovingRight = true;
            }

        }
        rockDropTimer -= Time.deltaTime;
        if (rockDropTimer <= 0)
        {
            DropRock();
            rockDropTimer = rockDropInterval; // Zresetuj licznik
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
    void DropRock()
    {
        Instantiate(rockPrefab, this.transform.position, Quaternion.identity);
        
    }

}
