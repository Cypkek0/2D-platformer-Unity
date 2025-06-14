using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KustoszKontroller : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 5.0f; // Pr�dko�� opadania
    [SerializeField] private float destroyHeight = -10.0f; // Wysoko��, przy kt�rej kamie� zostanie usuni�ty
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);

        if (transform.position.y <= destroyHeight)
        {
            Destroy(gameObject);
        }
    }
}
