using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollisionDetection : MonoBehaviour
{
    public Melee ME;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && ME.isAttacking)
        {
            other.GetComponent<Enemy>().TakeDamage(ME.AttackDmg);
            
        }
    }
}
