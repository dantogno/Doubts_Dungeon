using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public Transform firingPoint; // The 3D firing point transform
    public GameObject projectilePrefab; // The projectile prefab to instantiate
    public float projectileSpeed = 10f; // Speed of the projectile

    public float steadyFireRate = 0.5f;
    public float individualFireRate = 0.1f;
    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (Time.time >= nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + steadyFireRate; // Use steady fire rate when holding
            }
        }
        else
        {
            // Check if the "Fire1" button was clicked (not held)
            if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + individualFireRate; // Use individual fire rate for clicks
            }
        }
    }

    void Fire()
    {
        // Cast a ray from the mouse position into the game world
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Get the point where the ray hit a collider
            Vector3 targetPosition = hit.point;

            // Preserve the y-coordinate of the firing point
            targetPosition.y = firingPoint.position.y;

            // Calculate the direction only in the x and z plane, keeping y fixed
            Vector3 direction = (targetPosition - firingPoint.position);
            direction.y = 0; // Keep y-coordinate fixed (flat plane)

            // Normalize the direction
            direction.Normalize();

            // Create a new instance of the projectile
            GameObject projectile = Instantiate(projectilePrefab, firingPoint.position, Quaternion.identity);

            // Get the rigidbody of the projectile (assuming it has one)
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // Set the velocity of the projectile based on the calculated direction and speed
                rb.velocity = direction * projectileSpeed;
            }
            else
            {
                Debug.LogError("Projectile does not have a Rigidbody component!");
            }
        }
    }
}
