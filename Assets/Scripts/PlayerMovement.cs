using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climpSpeed = 5f;

    [Header("Death Kick")]
    [SerializeField] Vector2 deathKick = new Vector2(0, 10f);

    [Header("Weapon")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    [Header("Sound")]
    [SerializeField] AudioClip deathAudioClip;
    [SerializeField] float deathVolume = 0.5f;
    [SerializeField] AudioClip bounceAudioClip;
    [SerializeField] float bounceVolume = 0.6f;
    [SerializeField] AudioClip jumpAudioClip;
    [SerializeField] float jumpVolume = 0.4f;
    //[SerializeField] AudioClip climbAudioClip;
    //[SerializeField] float climbVolume = 0.2f;


    AudioSource audioSource;
    
    Vector2 moveInput;
    Rigidbody2D rigidbody2D;
    Animator animator;
    CapsuleCollider2D capsuleCollider2D;
    BoxCollider2D feetCollider2D;
    float gravityAtStart;
    bool isAlive = true;

    void Start()
    {

        audioSource = GetComponent<AudioSource>();


        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        feetCollider2D = GetComponent<BoxCollider2D>();
        gravityAtStart = rigidbody2D.gravityScale;
    }

    
    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        ClimpLadder();
        Bouncing();
        Die();
    }

    void Bouncing()
    {
        if (!isAlive) { return; }
        if (feetCollider2D.IsTouchingLayers(LayerMask.GetMask("Bouncing")))
        {
            audioSource.clip = bounceAudioClip;
            audioSource.volume = bounceVolume;
            audioSource.Play();
        }
    }

    void OnShoot(InputValue value)
    {
        if (!isAlive) { return; }
        if (value.isPressed)
        {
            animator.SetTrigger("Shooting");
            Instantiate(bullet, gun.position, transform.rotation);
        }
        
        
    }
    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
        
    }
    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        if (!feetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if (value.isPressed)
        {
            audioSource.clip = jumpAudioClip;
            audioSource.volume = jumpVolume;
            audioSource.Play();

            rigidbody2D.velocity += new Vector2(0f, jumpSpeed);
            animator.SetBool("isShooting", false);
        }
    }
    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, rigidbody2D.velocity.y); 
        rigidbody2D.velocity = playerVelocity;

        bool playerHasHorizotalSpeed = Mathf.Abs(rigidbody2D.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerHasHorizotalSpeed);
        animator.SetBool("isShooting", false);

    }
    void FlipSprite()
    {
        bool playerHasHorizotalSpeed = Mathf.Abs(rigidbody2D.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizotalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidbody2D.velocity.x), 1f);
        }
        
    }
    void ClimpLadder()
    {
        if (!feetCollider2D.IsTouchingLayers(LayerMask.GetMask("Climping")))
        {
            rigidbody2D.gravityScale = gravityAtStart;
            animator.SetBool("isClimbing", false);
            return;
        }
        Vector2 climpVelocity = new Vector2(rigidbody2D.velocity.x, moveInput.y*climpSpeed);
        rigidbody2D.velocity = climpVelocity;
        rigidbody2D.gravityScale = 0;

        //audioSource.clip = climbAudioClip;
        //audioSource.volume = climbVolume;
        //audioSource.Play();


        bool playerHasVerticalSpeed = Mathf.Abs(rigidbody2D.velocity.y) > Mathf.Epsilon;
        animator.SetBool("isClimbing", playerHasVerticalSpeed);
        animator.SetBool("isShooting", false);


    }
    void Die()
    {
        if (capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemies","Hazards")))
        {
            isAlive = false;
            audioSource.clip = deathAudioClip;
            audioSource.volume = deathVolume;
            audioSource.Play();
            animator.SetTrigger("Dying");
            rigidbody2D.velocity = deathKick;
            
            FindObjectOfType<GameSesion>().ProcessPlayerDeath();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "enemy")
        {
            Debug.Log("die on trigger exit");
            Destroy(collision.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            Debug.Log("die on collision enter");
            Destroy(collision.gameObject);
        }
    }

    
}
