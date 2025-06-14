using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    public float disappearDelay = 2f; 
    private Collider2D platformCollider;
    private SpriteRenderer platformRenderer;
    [SerializeField] private AudioClip crushsound;

    // Start is called before the first frame update
    void Start()
    {
        platformCollider = GetComponent<Collider2D>();
        platformRenderer = GetComponent<SpriteRenderer>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            
            Invoke(nameof(Disappear), disappearDelay);
        }
    }
    // Update is called once per frame
    void Disappear()
    {
        AudioSource.PlayClipAtPoint(crushsound, transform.position);
        


        platformCollider.enabled = false;
        platformRenderer.enabled = false;

        
        Invoke(nameof(ResetPlatform), disappearDelay);
    }
    public void ResetPlatform()
    {
        platformCollider.enabled = true;
        platformRenderer.enabled = true;
    }

}
