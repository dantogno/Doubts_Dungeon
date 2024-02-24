using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Trap : MonoBehaviour
{
    [SerializeField]
    private Player P;

 
    [SerializeField]
    private float TrapCooldownTime;

    [SerializeField]
    public Animator TrapAni;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            P.PlayerHasBeenHit();
        }
    }

}
