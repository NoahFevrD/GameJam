using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{

    public float boundLimit = 30;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EraseOfLimit();
    }

    void EraseOfLimit()
    {
        // X Axis Limit
        if(transform.position.x > boundLimit)
        {
            Destroy(gameObject);
        }

        if(transform.position.x < -boundLimit)
        {
            Destroy(gameObject);
        }

        // Z Axis Limit
        if(transform.position.z > boundLimit)
        {
            Destroy(gameObject);
        }

        if(transform.position.z < -boundLimit)
        {
            Destroy(gameObject);
        }
    }
}
