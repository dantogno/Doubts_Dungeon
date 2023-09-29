using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    //public GameObject projectile;
    //public GameObject player;


    //void Update()
    //{
    //    if (Input.GetButtonDown("Fire1"))
    //    {
    //        var position = player.transform.position;
    //        Instantiate(projectile,position,transform.rotation);
    //    }
    //}

    public GameObject projectilePrefab;
    public GameObject player;
    public Camera mainCamera;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            FireProjectile();
        }
    }

    void FireProjectile()
    {
        if (projectilePrefab != null && player != null && mainCamera != null)
        {
            // Get the mouse position in world space
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f; // Ensure the z-coordinate is appropriate for 2D

            // Calculate the direction from player to mouse position
            Vector3 direction = (mousePosition - player.transform.position).normalized;

            // Calculate the rotation based on the direction
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);

            // Spawn the projectile at the player's position with calculated rotation
            Instantiate(projectilePrefab, player.transform.position, rotation);
        }
    }
}
