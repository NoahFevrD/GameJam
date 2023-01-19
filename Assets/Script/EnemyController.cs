using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int enemyIndex = 1;
    public float speed = 0.10f;

    public int health = 3;
    private bool isAttacking = false;
    private bool isDead = false;

    private GameObject player;
    public GameObject enemyModel;
    public GameObject enemyBullet;
    public GameObject deathParticle;
    public GameObject biteHitbox;
    private Rigidbody enemyRb;
    private RoomController roomScript;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        roomScript = GameObject.Find("Room").GetComponent<RoomController>();
        animator = enemyModel.GetComponent<Animator>();
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Choose Which Enemy is being controlled
        if(enemyIndex == 1 && !isDead)
        {
            Enemy1Update();
        }
        if(enemyIndex == 2 && !isDead)
        {
            Enemy2Update();
        }

        if(health <= 0 && !isDead)
        {
            StartCoroutine(Death());
        }
    }

    void Enemy1Update()
    {
        // Move and Look toward Player
        Vector3 PlayerPos = new Vector3(player.transform.position.x, 0.5f, player.transform.position.z);
        Vector3 lookDirection = (transform.position - PlayerPos).normalized;
        transform.Translate(lookDirection * speed * Time.deltaTime);

        if(lookDirection != Vector3.zero)
        {
            enemyModel.transform.forward = -lookDirection;
        }

        // ShootBullet if not Attacking
        if(!isAttacking)
        {
            StartCoroutine(ShootBullet());
        }
    }

    void Enemy2Update()
    {
        // Move and Look toward Player
        Vector3 PlayerPos = new Vector3(player.transform.position.x, 0.5f, player.transform.position.z);
        Vector3 lookDirection = -(transform.position - PlayerPos).normalized;
        transform.Translate(lookDirection * speed * Time.deltaTime);

        if(lookDirection != Vector3.zero)
        {
            enemyModel.transform.forward = lookDirection;
        }
        Debug.Log(enemyModel.transform.rotation);

        // BiteHitbox Always in Front
        biteHitbox.transform.position = transform.position;
        biteHitbox.transform.rotation = enemyModel.transform.rotation;

        // Attack if not Attacking
        if(!isAttacking)
        {
            StartCoroutine(BitePlayer());
        }
    }

    IEnumerator ShootBullet()
    {
        isAttacking = true;
        yield return new WaitForSeconds(2.7f);
        animator.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(0.3f);
        Instantiate(enemyBullet, transform.position, enemyModel.transform.rotation);
        yield return new WaitForSeconds(0.8f);
        animator.SetBool("IsAttacking", false);
        yield return new WaitForSeconds(2.0f);
        isAttacking = false;
    }

    IEnumerator BitePlayer()
    {
        isAttacking = true;
        animator.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(0.3f);
        biteHitbox.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        biteHitbox.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("IsAttacking", false);
        yield return new WaitForSeconds(1.5f);
        isAttacking = false;
    }

    IEnumerator Death()
    {
        isDead = true;
        roomScript.enemyNumber --;
        animator.SetBool("IsDead", true);
        yield return new WaitForSeconds(0.8f);
        Instantiate(deathParticle, transform.position,deathParticle.transform.rotation);
        Destroy(gameObject);
    }
}
