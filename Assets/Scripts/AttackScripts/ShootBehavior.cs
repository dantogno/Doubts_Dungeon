using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBehavior : MonoBehaviour
{

    [SerializeField]
    public bool usingController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (usingController)
        {
            JoystickAim();
        }
    }

    public void JoystickAim()
    {
        Vector3 JoystickAimDirection = GetJoystickAimDirection();
        if (JoystickAimDirection.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(JoystickAimDirection, Vector3.up);
        }
    }

    public Vector3 GetJoystickAimDirection()
    {
        return getCameraRight() * Input.GetAxisRaw("RSHorizontal") + getCameraForward() * Input.GetAxisRaw("RSVertical");
    }

    Vector3 getCameraForward()
    {
        // Calculate movement direction based on camera's perspective
        Vector3 cameraForward = Camera.main.transform.forward;
        // Ignore the y-component to stay in the x-z plane
        cameraForward.y = 0f;
        cameraForward.Normalize();
        return cameraForward;
    }

    Vector3 getCameraRight()
    {
        // Calculate movement direction based on camera's perspective
        Vector3 cameraRight = Camera.main.transform.right;

        // Ignore the y-component to stay in the x-z plane
        cameraRight.y = 0f;
        cameraRight.Normalize();
        return cameraRight;
    }
}
