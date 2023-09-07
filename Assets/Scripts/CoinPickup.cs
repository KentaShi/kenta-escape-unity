using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSound;
    [SerializeField] int pointOfCoin = 100;

    bool isCollected = false;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isCollected)
        {
            isCollected = true;
            AudioSource.PlayClipAtPoint(coinPickupSound, Camera.main.transform.position);
            FindObjectOfType<GameSesion>().PickUp(pointOfCoin);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
