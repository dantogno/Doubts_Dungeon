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


    public void SpawnObject()
    {
        GameObject.Instantiate(gameObject);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OrbEffect.Stop();
            collider.enabled = false;
        }
    }
}
