using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kick : MonoBehaviour
{

    private float kickForce = 15.0f; 
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy1") && other.gameObject.GetComponent<Rigidbody>() != null)
        {
            Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();
            otherRb.AddForce(gameObject.transform.forward * kickForce, ForceMode.Impulse);
        }

        if(other.gameObject.CompareTag("Boss") && other.gameObject.GetComponent<Rigidbody>() != null)
        {
            BossController bossControllerScript = other.GetComponent<BossController>();
            bossControllerScript.health -= 5;
            bossControllerScript.slider.value = bossControllerScript.health;
        }
    }
}
