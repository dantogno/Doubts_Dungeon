using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public GameObject Sword;
    public bool CanAttack = true;
    public float AttackCoolDown = 0.1f;
    public Collider Hitbox;
    public bool isAttacking = false;
    public int AttackDmg;

 
    public void SwordAttack()
    {
        Sword.SetActive(true);
        CanAttack = false;
        isAttacking = true;
        Animator ani = Sword.GetComponent<Animator>();
        ani.SetTrigger("Attack");

        PlayerStateManager.instance.ActionState = ActionState.Attack;

        StartCoroutine(ResetAttackCooldown());
        
    }

    private void Start()
    {
        Sword.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if(!isAttacking) { PlayerStateManager.instance.ActionState = ActionState.Idle; }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (CanAttack)
            {
                SwordAttack();
            }
        }
    }

    IEnumerator ResetAttackCooldown()
    {
        StartCoroutine(ResetAttackBool());
        yield return new WaitForSeconds(AttackCoolDown);
        CanAttack = true;

        //PlayerStateManager.instance.ActionState = ActionState.Idle;
    }

    IEnumerator ResetAttackBool()
    {
        yield return new WaitForSeconds(0.8f);
        isAttacking = false;
    }
}
