using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovigPlatformControllerH : MonoBehaviour
{

    
    
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 0.1f; // Prêdkoœæ ruchu gracza
    private float startPositionY;
    [Range(0.01f, 20.0f)][SerializeField] private float moveRange = 1.0f;
    private bool isMovingUpwards = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingUpwards)
        {
            if (this.transform.position.y < startPositionY + moveRange)
            {
                MoveUpwards();
            }
            else
            {

                isMovingUpwards = false;
            }
        }
        else
        {
            if (this.transform.position.y > startPositionY - moveRange)
            {
                MoveDownwards();

            }
            else
            {
                isMovingUpwards = true;
            }

        }
    }

    void Awake()
    {
        startPositionY = this.transform.position.y;
      
    }
    
  
    void MoveUpwards()
    {
        transform.Translate( 0.0f, moveSpeed * Time.deltaTime, 0.0f, Space.World);
        
    }
    void MoveDownwards()
    {
        transform.Translate( 0.0f, moveSpeed * Time.deltaTime * -1, 0.0f, Space.World);
        
    }

}
