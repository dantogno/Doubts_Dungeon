using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    //private bool swing = false;
    //int degreee = 1;
    //private float weaponY = -0.3f;
    //private float weaponX = -0.3f;

    //Vector3 pos;
    //public GameObject player;

    //void Update() {
    //    UpdateWeaponPosition();

    //    if (Input.GetKey(KeyCode.Space))
    //    {
    //        Debug.Log("Space hit");
    //        GetComponent<SpriteRenderer>().enabled = true;
    //        transform.GetChild(0).gameObject.SetActive(true);
    //        Attack();
    //    }
    //}

    //private void FixedUpdate()
    //{
    //    if (swing) {
    //        degreee -=6;
    //        if(degreee < -100) {

    //            degreee = 0;
    //            swing = false;
    //            GetComponent<SpriteRenderer>().enabled = false;
    //            transform.GetChild(0).gameObject.SetActive(false);
    //        }

    //        if (player.GetComponent<CharacterScript>().turnedRight){
    //            transform.eulerAngles = Vector3.forward * degreee;
    //        }
    //        else { transform.eulerAngles = Vector3.back * degreee; }


    //    }
    //}

    //void Attack() {

    //    UpdateWeaponPosition();

    //    swing = true;

    //}

    //void UpdateWeaponPosition() {

    //    if (player.GetComponent<CharacterScript>().turnedRight)
    //    {
    //        GetComponent<SpriteRenderer>().flipX = true;
    //        weaponX = 0.3f;
    //    }
    //    else
    //    {
    //        GetComponent<SpriteRenderer>().flipX = false;
    //        weaponX = -0.3f;
    //    }
    //    pos = player.transform.position;
    //    pos.x += weaponX;
    //    pos.y += weaponY;
    //    transform.position = pos;
    //}

    private bool swing = false;
    private int degreee = 1;
    private float weaponY = -0.3f;
    private float weaponX = -0.3f;

    private Vector3 pos;
    public GameObject player;

    void Update()
    {
        UpdateWeaponPosition();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space hit");
            GetComponent<SpriteRenderer>().enabled = true;
            transform.GetChild(0).gameObject.SetActive(true);
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if (swing)
        {
            degreee -= 6;
            if (degreee < -100)
            {
                degreee = 0;
                swing = false;
                GetComponent<SpriteRenderer>().enabled = false;
                transform.GetChild(0).gameObject.SetActive(false);
            }

            if (player.GetComponent<CharacterScript>().turnedRight)
            {
                transform.eulerAngles = Vector3.forward * degreee;
            }
            else
            {
                transform.eulerAngles = Vector3.back * degreee;
            }
        }
    }

    void Attack()
    {
        UpdateWeaponPosition();
        swing = true;
    }

    void UpdateWeaponPosition()
    {
        if (player.GetComponent<CharacterScript>().turnedRight)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            weaponX = 0.3f;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
            weaponX = -0.3f;
        }
        pos = player.transform.position;
        pos.x += weaponX;
        pos.y += weaponY;
        transform.position = pos;
    }

}
