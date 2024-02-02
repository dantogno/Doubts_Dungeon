using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementNew : MonoBehaviour
{
    // P R O P E R T I E S

    [Header("Movement Settings")]
    [SerializeField] float Speed = 8f;
    [SerializeField] float rotationSpeed = 360.0f;
    [SerializeField] float dodgeDistance = 2.5f;
    [SerializeField] float dodgeDuration = 0.05f;
    [SerializeField] float decelerationFactor = 10.0f; // Adjust the value as needed

    [Header("Camera Properties")]
    [SerializeField]
    GameObject VirtualCamera;
    [SerializeField]
    float ShakeIntensity;
    [SerializeField]
    float totalShakeTime;
    float shakeTimer;

    CinemachineVirtualCamera CMVcam;

    [Header("Animation Settings")]
    [SerializeField] Animator playerAnimator;


    [Header("Misc")]
    Plane plane;

    [SerializeField]
    public bool usingController;


    // U N I T Y  M E T H O D S
    // Start is called before the first frame update
    void Start()
    {

        plane = new Plane(Vector3.down, transform.position.y);
        CMVcam = VirtualCamera.GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }


    // M I S C

    void LookAtMouse()
    {
        // turns the player to be looking at the mouse
    }
    void Move()
    {
        // moves the player 
    }

    void GetMoveVector()
    {
        //calculates the move vector for the player 
    }

    void Dodge()
    {
        // the dodge action
    }

    bool CanDodge()
    {
        // checks if player can dodge
    }
}
