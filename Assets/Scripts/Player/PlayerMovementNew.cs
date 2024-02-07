using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputMode { MouseAndKeyboard, Controller }
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

    

    [Header("Animation Settings")]
    [SerializeField] Animator playerAnimator;


    [Header("Misc")]
    Plane plane;

    [SerializeField]
    public InputMode PlayerInputMode;

    [Header("References")]
    public ShootBehavior SB;
    private Rigidbody rb;
    CinemachineVirtualCamera CMVcam;


    // U N I T Y  M E T H O D S
    // Start is called before the first frame update
    void Start()
    {
        plane = new Plane(Vector3.down, transform.position.y);
        CMVcam = VirtualCamera.GetComponent<CinemachineVirtualCamera>();
        playerAnimator = GetComponent<Animator>();
        SB = GetComponent<ShootBehavior>(); 
        rb = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
        HandleLookDirection();
    }


    // M I S C

    void HandleLookDirection()
    {
        switch(PlayerInputMode)
        {
            case InputMode.MouseAndKeyboard:
                LookAtMouse();
                break;

            case InputMode.Controller:

                break;
        }
    }
    void LookAtMouse()
    {
        // turns the player to be looking at the mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 targetPosition = ray.GetPoint(distance);

            // Preserve the y-coordinate of the player
            // Calculate the direction only in the x and z plane, keeping y fixed
            Vector3 direction = (targetPosition - transform.position);
            direction.y = 0; // Keep y-coordinate fixed (flat plane)

            if (direction != Vector3.zero)
            {
                // Calculate the angle between the forward direction and the mouse direction
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

                // Rotate the player around the Y-axis to face the mouse cursor
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                rb.MoveRotation(Quaternion.Euler(0f, angle, 0f));
            }
        }
    }
    void Move()
    {
        // moves the player 
        Vector3 move = GetMoveVector();
        playerAnimator.SetFloat("speed", move.magnitude);

        move *= Speed * Time.deltaTime;
        rb.AddForce(move, ForceMode.Force);

    }

    Vector3 GetMoveVector()
    {
        //calculates the move vector for the player 

        Vector3 move = Vector3.zero;

        switch (PlayerInputMode)
        {
            case InputMode.MouseAndKeyboard:
                move = (Input.GetAxis("Vertical") * getCameraForward()
                    + Input.GetAxis("Horizontal") * getCameraRight()).normalized;
                break;

            case InputMode.Controller:
                move = (Input.GetAxis("LSVertical") * getCameraForward()
                    + Input.GetAxis("LSHorizontal") * getCameraRight()).normalized;
                break;
        }

        return move;
        
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

    void Dodge()
    {
        // the dodge action
    }

    //bool CanDodge()
    //{
    //    // checks if player can dodge
    //}
}
