using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{

    public float startDelay = 4.0f;
    void Start()
    {
        InvokeRepeating("Destroy", startDelay, startDelay);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
