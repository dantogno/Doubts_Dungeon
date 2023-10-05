using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollisionScript : MonoBehaviour
{
    //[SerializeField]
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Check if the projectile is outside the camera bounds
        if (!IsInCameraBounds())
        {
            Destroy(gameObject);
        }
    }

    bool IsInCameraBounds()
    {
        if (mainCamera == null)
            return true; // Assume it's in bounds if the camera is not found

        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
        return (viewportPosition.x >= 0 && viewportPosition.x <= 1 && viewportPosition.y >= 0 && viewportPosition.y <= 1);
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
