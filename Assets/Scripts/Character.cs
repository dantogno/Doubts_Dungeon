using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    private CharacterController characterController;
    public float regularSpeed = 4f; // Default speed
    public float sprintSpeed = 8f; // Speed while sprinting
    public float sprintDuration = 2f; // Duration of sprint in seconds
    private float currentSprintTime = 0f;
    private bool isSprinting = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
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
        characterController.Move(move * Time.deltaTime * currentSpeed);

        LookAtMouse();

        //Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //characterController.Move(move * Time.deltaTime * Speed);

        //LookAtMouse();
    }

    private void LookAtMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10));

        Vector3 direction = mousePosition - transform.position;

        direction.y = 0;

        if (direction != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
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
