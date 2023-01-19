using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextRoom : MonoBehaviour
{

    private bool canChange = false;
    public GameObject eInteract;
    private PlayerController playerScript;
    public RoomController roomScript;


    void Start()
    {
        roomScript = GameObject.Find("Room").GetComponent<RoomController>();
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    // Update is called once per frame
    void Update()
    {
        if(canChange && playerScript.ePressed)
        {
            transform.position = new Vector3(15, 0.5f, 25);
            NextZone();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            canChange = true;
            eInteract.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            canChange = false;
            eInteract.SetActive(false);
        }
    }

    void NextZone()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
