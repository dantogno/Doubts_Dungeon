using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public GameObject projectile;
    public GameObject player;


    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            var position = player.transform.position;
            Instantiate(projectile,position,transform.rotation);
        }
    }
}
