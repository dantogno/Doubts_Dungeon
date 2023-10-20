using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public static event Action OnPlayerDeath;

    private Rigidbody rb;
    private PlayerMovement Player;
    public int health = 3;
    public int maxhealth = 3;
    public float regularSpeed = 4f; // Default speed
    public float sprintSpeed = 8f; // Speed while sprinting
    public float sprintDuration = 2f; // Duration of sprint in seconds
    private float currentSprintTime = 0f;
    private bool isSprinting = false;
    public float rotationSpeed = 360.0f;

    private bool canTakeDamage = true;
    public float damageCooldownDuration = 1f;

    public float decelerationFactor = 10.0f; // Adjust the value as needed

    Plane plane;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Player = GetComponent<PlayerMovement>();
        plane = new Plane(Vector3.down, transform.position.y);
    }

    void Update()
    {
        // Check if the player wants to sprint
        if (Input.GetButtonDown("Sprint") && !isSprinting)
        {
            StartSprint();
        }

        if (health == 0)
        {
            OnGameOver();
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

        //// Movement
        //float currentSpeed = isSprinting ? sprintSpeed : regularSpeed;
        ////Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        ////rb.velocity = move * currentSpeed;

        //// Calculate movement direction based on camera's perspective
        //Vector3 cameraForward = Camera.main.transform.forward;
        //Vector3 cameraRight = Camera.main.transform.right;

        //// Ignore the y-component to stay in the x-z plane
        //cameraForward.y = 0f;
        //cameraRight.y = 0f;
        //cameraForward.Normalize();
        //cameraRight.Normalize();

        //// Calculate the movement direction based on input and camera orientation
        //Vector3 move = (Input.GetAxis("Vertical") * cameraForward + Input.GetAxis("Horizontal") * cameraRight).normalized;

        //// Apply gravity to the movement
        //Vector3 gravityVector = Physics.gravity;
        //move += gravityVector * Time.deltaTime;


        //rb.velocity = move * currentSpeed;

        // Movement
        float currentSpeed = isSprinting ? sprintSpeed : regularSpeed;

        // Calculate movement direction based on camera's perspective
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        // Ignore the y-component to stay in the x-z plane
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate the movement direction based on input and camera orientation
        Vector3 move = (Input.GetAxis("Vertical") * cameraForward + Input.GetAxis("Horizontal") * cameraRight).normalized;

        // Apply gravity to the movement
        Vector3 gravityVector = Physics.gravity;
        move += gravityVector * Time.deltaTime;

        if (move.magnitude > 0)
        {
            // Player is providing input, move them at the current speed
            rb.velocity = move * currentSpeed;
        }
        else
        {
            // No input provided, apply constant deceleration to gradually stop the player
            rb.velocity -= rb.velocity.normalized * decelerationFactor * Time.deltaTime;

            // Ensure the velocity doesn't go negative
            rb.velocity = Vector3.Max(rb.velocity, Vector3.zero);
        }

        LookAtMouse();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            if (canTakeDamage)
            {
                canTakeDamage = false;
                StartCoroutine(DamageCooldown());
                // Reduce player's health when colliding with an enemy
                TakeDamage(1);
            }

        }
    }

    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(damageCooldownDuration);
        canTakeDamage = true; // Allow taking damage again after cooldown
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        canTakeDamage = true;

        // Check for player death
        if (health == 0)
        {
            OnGameOver();
        }
    }

    public void OnGameOver()
    {
        OnPlayerDeath?.Invoke();
    }

    private void OnEnable()
    {
        PlayerMovement.OnPlayerDeath += DisablePlayer;
    }

    private void OnDisable()
    {
        PlayerMovement.OnPlayerDeath -= DisablePlayer;
    }

    public void DisablePlayer()
    {
        Player.enabled = false;
    }

    Vector3 targPos;
    private void LookAtMouse()
    {
        // Cast a ray from the mouse position into the game world
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(plane.Raycast(ray, out float distance))
        {
            Vector3 targetPosition = ray.GetPoint(distance);
            targPos = targetPosition;

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
            }
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(targPos, 0.1f);
        Debug.DrawLine(transform.position, targPos, Color.cyan);
    }

}
