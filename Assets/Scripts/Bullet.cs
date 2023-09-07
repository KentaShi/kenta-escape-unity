using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] AudioClip shootAudioClip;

    AudioSource audioSource;
    Rigidbody2D rigidbody2D;
    PlayerMovement player;
    float xSpeed;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = shootAudioClip;
        audioSource.Play();

        rigidbody2D = GetComponent<Rigidbody2D>(); 
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    
    void Update()
    {
        rigidbody2D.velocity = new Vector2(xSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }
        if (gameObject != null)
        {
            Destroy(gameObject);

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject != null)
        {
            Destroy(gameObject);

        }
    }

}
