using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    //private CharacterController characterController;
    //public float regularSpeed = 4f; // Default speed

    //public float sprintSpeed = 8f; // Speed while sprinting
    //public float sprintDuration = 2f; // Duration of sprint in seconds
    //private float currentSprintTime = 0f;
    //private bool isSprinting = false;

    //public float rotationSpeed = 360.0f;

    //void Start()
    //{
    //    characterController = GetComponent<CharacterController>();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    // Check if the player wants to sprint
    //    if (Input.GetButtonDown("Sprint") && !isSprinting)
    //    {
    //        StartSprint();
    //    }

    //    // Check if the sprint duration has passed
    //    if (isSprinting)
    //    {
    //        currentSprintTime += Time.deltaTime;
    //        if (currentSprintTime >= sprintDuration)
    //        {
    //            StopSprint();
    //        }
    //    }

    //    // Movement
    //    float currentSpeed = isSprinting ? sprintSpeed : regularSpeed;
    //    Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    //    characterController.Move(move * Time.deltaTime * currentSpeed);

    //    LookAtMouse();
    //}

    //private void LookAtMouse()
    //{
    //    Vector3 mousePosition = Input.mousePosition;
    //    mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10));

    //    // Ensure the player's Y position remains unchanged

    //    Vector3 direction = mousePosition - transform.position;

    //    if (direction != Vector3.zero)
    //    {
    //        // Calculate the angle between the forward direction and the mouse direction
    //        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

    //        // Rotate the player around the Y-axis to face the mouse cursor
    //        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    //    }
    //}

    //private void StartSprint()
    //{
    //    isSprinting = true;
    //    currentSprintTime = 0f;
    //}

    //private void StopSprint()
    //{
    //    isSprinting = false;
    //    // You can add any post-sprint logic here, if needed.
    //}

    private Rigidbody rb;
    public float regularSpeed = 4f; // Default speed
    public float sprintSpeed = 8f; // Speed while sprinting
    public float sprintDuration = 2f; // Duration of sprint in seconds
    private float currentSprintTime = 0f;
    private bool isSprinting = false;
    public float rotationSpeed = 360.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player wants to sprint
        if (Input.GetButtonDown("Sprint") && !isSprinting)
        {
            StartSprint();
        }

        // Check if the sprint duration has passed
        if (isSprinting)
        {
            currentSprintTime += Time.deltaTime;
            if (currentSprintTime >= sprintDuration)
            {
                StopSprint();
            }
        }

        // Movement
        float currentSpeed = isSprinting ? sprintSpeed : regularSpeed;
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rb.velocity = move * currentSpeed;

        LookAtMouse();
    }

    private void LookAtMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10));

        // Ensure the player's Y position remains unchanged
        Vector3 direction = mousePosition - transform.position;

        if (direction != Vector3.zero)
        {
            // Calculate the angle between the forward direction and the mouse direction
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // Rotate the player around the Y-axis to face the mouse cursor
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    private void StartSprint()
    {
        isSprinting = true;
        currentSprintTime = 0f;
    }

    private void StopSprint()
    {
        isSprinting = false;
        // You can add any post-sprint logic here, if needed.
    }
}
