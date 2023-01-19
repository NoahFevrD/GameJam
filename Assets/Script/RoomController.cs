using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public float startPosX = 15;
    public float startPosZ = 5;

    public float smallBound = 0.75f;
    public float largeBound = 29.25f;

    public int enemyNumber = 0;
    public bool hasBoss = false;
    public bool BossIsDead = false;
    private bool finished = false;
    public GameObject nextRoom;
    public GameObject[] toggleGameobject;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hasBoss && BossIsDead && !finished)
        {
            finished = true;
            SpawnAndDispawnGameObject();
        }

        if(!hasBoss && enemyNumber <= 0 && !finished)
        {
            finished = true;
            SpawnAndDispawnGameObject();
        }
    }

    void SpawnAndDispawnGameObject()
    {
        nextRoom.SetActive(true);
        for(int index = 0;index < toggleGameobject.Length; index++)
        {
            toggleGameobject[index].SetActive(!toggleGameobject[index].activeInHierarchy);
        }
    }
}
