using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovementCC : MonoBehaviour
{
    public static event Action OnPlayerDeath;
    private CharacterController characterController;
    private PlayerMovement Player;
    public int health = 3;
    public int maxhealth;
    public float regularSpeed = 4f; // Default speed
    public float sprintSpeed = 8f; // Speed while sprinting
    public float sprintDuration = 2f; // Duration of sprint in seconds
    private float currentSprintTime = 0f;
    private bool isSprinting = false;
    public float rotationSpeed = 360.0f;

    private float gravity = -.81f;

    private bool canTakeDamage = true;
    public float damageCooldownDuration = 1f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Player = GetComponent<PlayerMovement>();
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

        // Apply Gravity
        ApplyGravity();

        // Movement
        float currentSpeed = isSprinting ? sprintSpeed : regularSpeed;

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = (Input.GetAxis("Vertical") * cameraForward + Input.GetAxis("Horizontal") * cameraRight).normalized;
        Vector3 movement = moveDirection * currentSpeed;

        characterController.Move(movement * Time.deltaTime);

        LookAtMouse();
    }

    private void ApplyGravity()
    {
        // Apply gravity if not grounded
        if (!characterController.isGrounded)
        {
            Vector3 gravityVector = Vector3.up * gravity;
            characterController.Move(gravityVector * Time.deltaTime);
        }
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

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = hit.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Calculate direction from the enemy to the player
                Vector3 direction = transform.position - hit.transform.position;
                direction.Normalize();

                // Calculate the knockback position
                Vector3 knockbackPosition = hit.transform.position + direction * enemy.knockbackDistance;

                // Trigger knockback in the enemy
                //enemy.TriggerKnockback(knockbackPosition, enemy.knockbackDuration);
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
        if (health <= 0)
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

    private void LookAtMouse()
    {
        // Cast a ray from the mouse position into the game world
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Get the point where the ray hit a collider
            Vector3 targetPosition = hit.point;

            // Preserve the y-coordinate of the player
            targetPosition.y = transform.position.y;

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
}
