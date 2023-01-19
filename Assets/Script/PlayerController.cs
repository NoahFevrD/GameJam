using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    private float speed = 5;
    private float jumpForce = 5;

    private bool bulletReady = true;
    private float timeBetweenAttack = 0.3f;
    private bool kickReady = true;
    private float timeBetweenKick = 5.5f;

    public int health = 1;
    public int life = 3;
    public bool isInvunerable= false;
    private bool isDead = false;
    public bool gameOver = false;
    public bool ePressed = false;
    private Camera mainCamera;
    
    private Rigidbody playerRb;
    private RoomController roomScript;
    private Animator animator;
    private Vector3 oldPos;
    public GameObject character;
    private SkinnedMeshRenderer charMesh1;
    private SkinnedMeshRenderer charMesh2;
    public GameObject bulletPrefab;
    public GameObject kickHitBox;
    public GameObject minusOneLife;
    private Rigidbody kickRb;
    private bool isTeleporting = false;
    public GameObject nextRoom;

    // Start is called before the first frame update
    void Start()
    {
        roomScript = GameObject.Find("Room").GetComponent<RoomController>();
        oldPos = transform.position;
        animator = character.GetComponent<Animator>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        playerRb = gameObject.GetComponent<Rigidbody>();
        charMesh1 = GameObject.Find("Ch_09").GetComponent<SkinnedMeshRenderer>();
        charMesh2 = GameObject.Find("Ch_09_glass").GetComponent<SkinnedMeshRenderer>();
        transform.position = new Vector3(roomScript.startPosX, transform.position.y, roomScript.startPosZ);
        NextMap();
    }

    // Update is called once per frame
    void Update()
    {
        // Play Dead Animation
        animator.SetBool("isDead", isDead);

        if(life > 0 && !isDead)
        {
        // Move Character Around
        // Forward and Backward
        float verticalinput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * verticalinput * speed * Time.deltaTime);
        // Left and Right
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);

        // If E Key Down, Interact
        if(Input.GetKeyDown(KeyCode.E) && !ePressed)
        {
            ePressed = true;
        }

        if(Input.GetKeyUp(KeyCode.E) && ePressed)
        {
            ePressed = false;
        }

        // Running Animation when moving
        oldPos.y = transform.position.y;
        bool isRunning = animator.GetBool("isRunning");
        if(!isRunning && oldPos != transform.position)
        {
            animator.SetBool("isRunning", true);
        }
        if(isRunning && oldPos == transform.position)
        {
            animator.SetBool("isRunning", false);
        }
        oldPos = transform.position;

        // Jump
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //}

        // Turn Camera Around With Mouse
        LookAtMouse();
        
        // Shoot Bullet with Left Click
        float leftClick = Input.GetAxis("LeftFire");
        if(leftClick > 0)
        {
            ShootBullet();
        }

        // Kick Forward with Right Click
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            KickForward();
        }
        }

        // Limit the Character Mouvement at The limit of the Room
        if(!isTeleporting)
        {
            OutOfBound();
        }

        // Keep Kick Hit Box in front of the Player
        MaintainKickHitBoxForward();

        // If health lower or equal than 0, kill Player
        if(health <= 0 && !isDead)
        {
            Death();
        }
    }

    public void NextMap()
    {
        isTeleporting = true;
        roomScript = GameObject.Find("Room").GetComponent<RoomController>();
        StartCoroutine(BecomeImmune());
    }

    IEnumerator BecomeImmune()
    {
        float invunerableDuration = 2.0f;
        isInvunerable = true;
        StartCoroutine(FlickerPlayer(invunerableDuration));
        Vector3 telPos = new Vector3(15, 0.5f, 3);
        transform.position = telPos;
        isTeleporting = false;
        yield return new WaitForSeconds(invunerableDuration);
        isInvunerable = false;
    }

    void OutOfBound()
    {
        if(transform.position.x < roomScript.smallBound)
        {
            Vector3 x = new Vector3(roomScript.smallBound, transform.position.y,transform.position.z);
            transform.position = x;
        }

        if(transform.position.z < roomScript.smallBound)
        {
            Vector3 z = new Vector3(transform.position.x, transform.position.y,roomScript.smallBound);
            transform.position = z;
        }

        if(transform.position.x > roomScript.largeBound)
        {
            Vector3 x = new Vector3(roomScript.largeBound, transform.position.y,transform.position.z);
            transform.position = x;
        }

        if(transform.position.z > roomScript.largeBound)
        {
            Vector3 z = new Vector3(transform.position.x, transform.position.y,roomScript.largeBound);
            transform.position = z;
        }
    }

    void ShootBullet()
    {
        if(bulletReady)
        {
            bulletReady = false;
            StartCoroutine(Firerate());
            Vector3 spawnOffset = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            Instantiate(bulletPrefab, spawnOffset, character.transform.rotation);
        }
    }

    void KickForward()
    {
        if(kickReady)
        {
            kickReady = false;
            animator.SetBool("isKicking", true);
            StartCoroutine(KickInterval());
            StartCoroutine(KickDuration());

        }
    }

    void MaintainKickHitBoxForward()
    {
        kickHitBox.transform.position = character.transform.position;
        kickHitBox.transform.rotation = character.transform.rotation;
    }

    void LookAtMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            Vector3 target = hitInfo.point;
            target.y = character.transform.position.y;
            character.transform.LookAt(target);
        }
    }

    void Death()
    {
        isDead = true;
        Debug.Log("Player Dead");

        life -= 1;
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 0.5f,transform.position.z);
        Instantiate(minusOneLife, spawnPos,minusOneLife.transform.rotation);

        if(life > 0)
        {
            StartCoroutine(OneMoreLife());
        }
        else
        {
            StartCoroutine(GameOver());
        }
    }

    IEnumerator OneMoreLife()
    {
        float invunerableDuration = 4.0f;
        yield return new WaitForSeconds(3.0f);
        isDead = false;
        isInvunerable = true;
        health = 1;
        StartCoroutine(FlickerPlayer(invunerableDuration));
        transform.position = new Vector3(roomScript.startPosX, 0.5f, roomScript.startPosZ);
        yield return new WaitForSeconds(invunerableDuration);
        isInvunerable = false;
    }

    IEnumerator FlickerPlayer(float invunerableDuration)
    {
        float timeBetweenFlicker = 0.125f;
        for(float i = 0;i < invunerableDuration;i += timeBetweenFlicker * 2)
        {
            yield return new WaitForSeconds(timeBetweenFlicker);
            charMesh1.enabled = false;
            charMesh2.enabled = false;
            yield return new WaitForSeconds(timeBetweenFlicker);
            charMesh1.enabled = true;
            charMesh2.enabled = true;
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(4.0f);
        SceneManager.LoadScene(0);
    }

    IEnumerator KickDuration()
    {
        yield return new WaitForSeconds(0.5f);
        kickHitBox.SetActive(true);
        yield return new WaitForSeconds(timeBetweenAttack);
        kickHitBox.SetActive(false);
        yield return new WaitForSeconds(1.2f);
        animator.SetBool("isKicking", false);
    }

    IEnumerator KickInterval()
    {
        yield return new WaitForSeconds(timeBetweenKick);
        kickReady = true;
    }

    IEnumerator Firerate()
    {
        yield return new WaitForSeconds(timeBetweenAttack);
        bulletReady = true;
    }
    
}
