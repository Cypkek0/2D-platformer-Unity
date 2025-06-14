using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingPlatform : MonoBehaviour
{
    public float visibleTime = 2f; 
    public float invisibleTime = 2f;
    public float delayBeforeDisappearing = 1f;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D platformCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<BoxCollider2D>();
        StartCoroutine(PlatformCycle()); 
    }

    IEnumerator PlatformCycle()
    {
        yield return new WaitForSeconds(delayBeforeDisappearing);
        while (true)
        {
            
            spriteRenderer.enabled = true;
            platformCollider.enabled = true;
            yield return new WaitForSeconds(visibleTime);

            
            spriteRenderer.enabled = false;
            platformCollider.enabled = false;
            yield return new WaitForSeconds(invisibleTime);
        }
    }
}
