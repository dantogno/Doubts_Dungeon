using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int healthamount;
    private Player playerref;

    [SerializeField]
    public AudioSource bottleSource;
    public AudioClip pickupSound;
    public float volume = 1f;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerref = other.GetComponent<Player>();
            HealthPickupGain();
        }
    }

    public void HealthPickupGain()
    {
        playerref.PickupHealth();
        Destroy(gameObject);
        bottleSource.PlayOneShot(pickupSound, volume);
    }
}
