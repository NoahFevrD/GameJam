using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{

    public int health = 70;
    private bool isDead = false;

    private GameObject player;
    public GameObject boss;
    public GameObject babyBoss;
    public GameObject enemyBullet;
    public GameObject deathParticle;
    public Slider slider;

    private RoomController roomScript;
    private Animator animator;
    public int numberOfAttack;

    public float attackDelay;
    private bool isAttacking = false;

    public bool bossController1 = true;
    public bool bossController2 = false;
    public GameObject smashHitbox;
    public GameObject stabHitbox;

    public bool bossController3 = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = boss.GetComponent<Animator>();
        roomScript = GameObject.Find("Room").GetComponent<RoomController>();
        player = GameObject.Find("Player");
        MaxHealth();
    }

    void MaxHealth()
    {
        slider.maxValue = health;
        slider.value = health;
    }

    // Update is called once per frame
    void Update()
    {

        // If Boss is not Dead
        if(!isDead)
        {
            // Look At Player
            Vector3 lookAt = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            boss.transform.LookAt(lookAt);

            // Select Random Attack from Boss
            if(!isAttacking)
            {
                StartCoroutine(RandomAttack());
            }
            

        }
        // If Boss has less than 0 Health, Kill the Boss
        if(health <= 0 && !isDead)
        {
            Death();
        }
    }

        IEnumerator RandomAttack()
    {
        isAttacking = true;
        int randomIndex = Random.Range(1, numberOfAttack + 1);
        StartCoroutine(BossAttack(randomIndex));

        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }

    void Death()
    {
        isDead = true;
        animator.SetBool("IsDead", isDead);
        roomScript.BossIsDead = true;
        if(bossController1)
        {
            GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy1");
            for(int i = 0;i < enemy.Length;i ++)
            {
                EnemyController enemyScript = enemy[i].GetComponent<EnemyController>();
                enemyScript.health = 0;
            }
        }
        if(bossController2)
        {
            GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy2");
            for(int i = 0;i < enemy.Length;i ++)
            {
            EnemyController enemyScript = enemy[i].GetComponent<EnemyController>();
            enemyScript.health = 0;
            }
        StartCoroutine(Explosion());
        }
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(2.4f);
        Instantiate(deathParticle, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    IEnumerator BossAttack(int attackNumber)
    {
        if(bossController1)
        {
            if(attackNumber == 1 )
            {
                for(int i = 0; i < 4 ; i++)
                {
                    if(!isDead){
                    animator.SetBool("IsAttacking", true);
                    for(float yRotation = boss.transform.rotation.y;yRotation <= boss.transform.rotation.y + 360; yRotation += 15)
                    {   
                        Vector3 spawnPos = new Vector3(transform.position.x, 0.5f, transform.position.z);
                        Vector3 spawnRotation = new Vector3(boss.transform.rotation.x, yRotation, boss.transform.rotation.z);
                        Instantiate(enemyBullet, spawnPos, Quaternion.Euler(spawnRotation));
                        yield return new WaitForSeconds(0.05f);
                    }
                    animator.SetBool("IsAttacking", false);
                    yield return new WaitForSeconds(1.5f);
                }}
            }

            else
            {
                animator.SetBool("IsAttacking", true);
                if(roomScript.enemyNumber < 1)
                {
                    animator.SetBool("IsAttacking", true);
                    Vector3 spawnPos = new Vector3(transform.position.x -5, 0.5f, transform.position.z);
                    for(int spawnPosZ = -3;spawnPosZ <= 3;spawnPosZ += 6)
                    {
                        spawnPos.z += spawnPosZ;
                        Instantiate(babyBoss, spawnPos, transform.rotation);
                        roomScript.enemyNumber ++;
                    }
                    spawnPos = new Vector3(transform.position.x +5, 0.5f, transform.position.z);
                    for(int spawnPosZ = -3;spawnPosZ <= 3;spawnPosZ += 6)
                    {
                        spawnPos.z += spawnPosZ;
                        Instantiate(babyBoss, spawnPos, transform.rotation);
                        roomScript.enemyNumber ++;
                    }
                }
                yield return new WaitForSeconds(0.5f);
                animator.SetBool("IsAttacking", false);
                
            }
        }

        if(bossController2)
        {
            if(attackNumber == 1 )
            {
                animator.SetBool("IsSpelling", true);
                if(roomScript.enemyNumber < 1)
                {
                    animator.SetBool("IsSpelling", true);
                    Vector3 spawnPos = new Vector3(transform.position.x -5, 0.5f, transform.position.z);
                    for(int spawnPosZ = -3;spawnPosZ <= 3;spawnPosZ += 6)
                    {
                        spawnPos.z += spawnPosZ;
                        Instantiate(babyBoss, spawnPos, transform.rotation);
                        roomScript.enemyNumber ++;
                    }
                    spawnPos = new Vector3(transform.position.x +5, 0.5f, transform.position.z);
                    for(int spawnPosZ = -3;spawnPosZ <= 3;spawnPosZ += 6)
                    {
                        spawnPos.z += spawnPosZ;
                        Instantiate(babyBoss, spawnPos, transform.rotation);
                        roomScript.enemyNumber ++;
                    }
                }
                yield return new WaitForSeconds(0.5f);
                animator.SetBool("IsSpelling", false);
            }

            if(attackNumber == 2)
            {
                isAttacking = true;
                animator.SetBool("IsStabbing", true);
                yield return new WaitForSeconds(0.5f);
                stabHitbox.SetActive(true);
                yield return new WaitForSeconds(0.3f);
                stabHitbox.SetActive(false);
                yield return new WaitForSeconds(0.2f);
                animator.SetBool("IsStabbing", false);
            }

            if(attackNumber == 3)
            {
                isAttacking = true;
                animator.SetBool("IsSmashing", true);
                yield return new WaitForSeconds(1.1f);
                stabHitbox.SetActive(true);
                yield return new WaitForSeconds(0.4f);
                stabHitbox.SetActive(false);
                yield return new WaitForSeconds(0.2f);
                animator.SetBool("IsSmashing", false);
            }
        }
    }
}

