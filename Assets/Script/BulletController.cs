using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float bulletSpeed = 30;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(gameObject.CompareTag("Bullet1") && !other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("Bite"))
        {
            if(other.gameObject.CompareTag("Enemy1") && other.gameObject.GetComponent<Rigidbody>() != null)
            {
                EnemyController enemyControllerScript = other.GetComponent<EnemyController>();
                enemyControllerScript.health -= 1;
                Destroy(gameObject);
            }

            if(other.gameObject.CompareTag("Boss") && other.gameObject.GetComponent<Rigidbody>() != null)
            {
                BossController bossControllerScript = other.GetComponent<BossController>();
                bossControllerScript.health -= 1;
                bossControllerScript.slider.value = bossControllerScript.health;
                Destroy(gameObject);
            }
        }

        if(gameObject.CompareTag("BulletE1") && other.GetComponent<PlayerController>() != null)
        {
            PlayerController playerControllerScript = other.GetComponent<PlayerController>();
            if(other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<Rigidbody>() != null && !playerControllerScript.isInvunerable)
            {
                playerControllerScript.health -= 1;
                Destroy(gameObject);
            }
        }

        if(gameObject.CompareTag("Bite") && other.GetComponent<PlayerController>() != null)
        {
            PlayerController playerControllerScript = other.GetComponent<PlayerController>();
            if(other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<Rigidbody>() != null && !playerControllerScript.isInvunerable)
            {
                playerControllerScript.health -= 1;
            }
        }

        if(gameObject.CompareTag("SmashHit") && other.GetComponent<PlayerController>() != null)
        {
            PlayerController playerControllerScript = other.GetComponent<PlayerController>();
            if(other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<Rigidbody>() != null && !playerControllerScript.isInvunerable)
            {
                playerControllerScript.health -= 1;
            }
        }

        if(gameObject.CompareTag("StabHit") && other.GetComponent<PlayerController>() != null)
        {
            PlayerController playerControllerScript = other.GetComponent<PlayerController>();
            if(other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<Rigidbody>() != null && !playerControllerScript.isInvunerable)
            {
                playerControllerScript.health -= 1;
            }
        }
    }
}
