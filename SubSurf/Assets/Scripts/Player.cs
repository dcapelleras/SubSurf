using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    [SerializeField] Transform leftTransform;
    [SerializeField] Transform rightTransform;
    [SerializeField] Transform centerTransform;

    public enum position { left, right, center }
    public position currentPos = position.center;

    public bool gameStarted;

    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpForce = 5f;

    Rigidbody rb;
    Animator anim;

    [SerializeField] bool isEnemy;

    int lives;
    [SerializeField] List<GameObject> livesImages = new List<GameObject>();

    [SerializeField] float immuneDuration;
    float immuneTimer;
    bool isImmune;

    public bool isJumping;

    bool jumped;
    bool falling;
    bool landed;
    float jumpCooldownTime = 0.7f;

    bool dead;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        lives = livesImages.Count;
        if (!isEnemy)
        {
            anim.SetBool("Running", true);
        }
    }

    private void Update()
    {
        /*if (!gameStarted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                gameStarted= true;
                RoadManager.Instance.GameStarted = true;
                anim.SetBool("Running", true);
            }
            return;
        }*/
        

        var step = moveSpeed * Time.deltaTime; // calculate distance to move

        if (RoadManager.Instance.gameOver)
        {
            return;
        }

        if (isImmune)
        {
            immuneTimer += Time.deltaTime;
            if (immuneTimer> immuneDuration)
            {
                isImmune = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            if (dead)
            {
                return;
            }
            anim.SetTrigger("Jump");
            rb.velocity = Vector3.up * jumpForce * 2f;
            jumped = true;
            landed = false;
            isJumping = true;
        }

        if (isJumping)
        {
            return;
        }
        
        switch (currentPos)
        {
            case position.left:
                transform.position = Vector3.MoveTowards(transform.position, leftTransform.position, step);
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    currentPos = position.center;
                }
                break;
            case position.right:
                transform.position = Vector3.MoveTowards(transform.position, rightTransform.position, step);
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    currentPos = position.center;
                }
                break;
            case position.center:
                transform.position = Vector3.MoveTowards(transform.position, centerTransform.position, step);
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    currentPos = position.left;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    currentPos = position.right;
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        if (dead)
        {
            return;
        }
        if (jumped)
        {
            jumped= false;
            rb.velocity = Vector3.up * jumpForce;
            StartCoroutine(JumpCooldown());
            rb.useGravity = true;
        }
        else if (falling)
        {
            rb.velocity -= Vector3.up * jumpForce * Time.deltaTime;
        }
        else if (landed)
        {
            rb.velocity = Vector3.zero;
            rb.useGravity= false;
        }
    }

    IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        jumped = false;
        yield return new WaitForSeconds(0.1f);
        falling= true;
        yield return new WaitForSeconds(jumpCooldownTime);
        landed= true;
        falling = false;
        isJumping = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isEnemy)
        {
            if (other.CompareTag("Player"))
            {
                anim.SetTrigger("Crash");
            }
            return;
        }
        if (other.CompareTag("Immune"))
        {
            isImmune = true;
            immuneTimer = 0;
            other.gameObject.SetActive(false);
        }
        if (isImmune)
        {
            return;
        }
        if (other.CompareTag("Obstacle"))
        {
            lives--;
            livesImages[lives].SetActive(false);
            
            if (lives <= 0)
            {
                dead = true;
                RoadManager.Instance.gameOver = true;
                rb.velocity = -transform.forward * RoadManager.Instance.moveSpeed;
                rb.useGravity = false;
            }
        }
        else if (other.CompareTag("Finish"))
        {
            Time.timeScale = 1f;
            RoadManager.Instance.gameFinished= true;
            RoadManager.Instance.GameOver();
            anim.SetTrigger("Fall");
            rb.velocity = Vector3.zero;
        }
    }
}
