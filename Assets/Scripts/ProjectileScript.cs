using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public Transform firingPoint; // The 3D firing point transform
    public GameObject projectilePrefab; // The projectile prefab to instantiate
    public float projectileSpeed = 10f; // Speed of the projectile

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Change to your input method
        {
            Fire();
        }
    }

    void Fire()
    {
        // Create a new instance of the projectile
        GameObject projectile = Instantiate(projectilePrefab, firingPoint.position, Quaternion.identity);

        // Get the rigidbody of the projectile (assuming it has one)
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Set the velocity of the projectile in the Z direction (forward)
            rb.velocity = firingPoint.forward * projectileSpeed;
        }
        else
        {
            Debug.LogError("Projectile does not have a Rigidbody component!");
        }
    }

    
}
