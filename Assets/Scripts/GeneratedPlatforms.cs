using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedPlatforms : MonoBehaviour
{

    [SerializeField] private GameObject platformPrefab;
    private static int PLATFORMS_NUM = 5;
    private GameObject[] platforms;
    private Vector2[] positions;

    void Awake()
    { 
        platforms = new GameObject[PLATFORMS_NUM];
        positions = new Vector2[PLATFORMS_NUM];

        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            positions[i] = new Vector2 (i+4, i+4);
            platforms[i] = Instantiate(platformPrefab, positions[i], Quaternion.identity);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
