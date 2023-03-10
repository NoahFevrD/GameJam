using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerControllerScript = other.gameObject.GetComponent<PlayerController>();
        if(other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<Rigidbody>() != null && !playerControllerScript.isInvunerable)
        {
            playerControllerScript.health -= 1;
        }
    }
}
