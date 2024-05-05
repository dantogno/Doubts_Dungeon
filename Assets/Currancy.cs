using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Currancy : MonoBehaviour
{
    public static event Action OnEnemyDeath;

    [SerializeField]
    VisualEffect OrbEffect;
 

    [SerializeField]
    private Collider collider;

    [SerializeField]
    private int CurrancyAmount;

    [SerializeField]
    public AudioSource orbSource;
    public AudioClip pickupSound;
    public float volume = 1f;


    private void Start()
    {

    }

    public void SpawnObject()
    {
        GameObject.Instantiate(gameObject);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OrbEffect.playRate = 3;
            OrbEffect.Stop();
            orbSource.PlayOneShot(pickupSound, volume);
            collider.enabled = false;
            other.gameObject.GetComponent<Player>().Currancy += CurrancyAmount;
        }
    }
}
