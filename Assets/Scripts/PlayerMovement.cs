using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class PlayerMovement : MonoBehaviour
{
    PlayerStateManager stateManager;

    public static event Action OnPlayerDeath;

    private PlayerMovement Player;

    public int health = 3;
    public int maxhealth = 3;

    public StaminaScript Stamina;

    public float regularSpeed = 4f; // Default speed
    public float decelerationSpeed = 4f;
    public float sprintSpeed = 8f; // Speed while sprinting
    public float sprintDuration = 2f; // Duration of sprint in seconds
    private float currentSprintTime = 0f;
    private bool isSprinting = false;

    public float rotationSpeed = 360.0f;

    public float dodgeDistance = 2.5f;
    public float dodgeDuration = 0.05f;

    private bool canTakeDamage = true;
    public float damageCooldownDuration = 1f;

    public float decelerationFactor = 10.0f; // Adjust the value as needed

    Plane plane;
    void Start()
    {
        Player = GetComponent<PlayerMovement>();
        plane = new Plane(Vector3.down, transform.position.y);
        stateManager = PlayerStateManager.instance;
        //this.ActionState = ActionState.Default;
    }

    void Update()
    {
        if (health == 0)
        {
            OnGameOver();
        }

        // Check if the player wants to sprint
        CheckForSprint();

        CheckForDodge();

        RecoverStamina();

        LookAtMouse();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    public void Movement()
    {
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
        Vector3 move = (Input.GetAxisRaw("Vertical") * cameraForward + Input.GetAxisRaw("Horizontal") * cameraRight).normalized;

        // Apply gravity to the movement
        Vector3 gravityVector = Physics.gravity;
        move += gravityVector * Time.deltaTime;

        if (move.magnitude > 0)
        {
            // Player is providing input, move them at the current speed
            Vector3 newPosition = transform.position + move * currentSpeed * Time.deltaTime;
            transform.position = newPosition;
        }
        else
        {
            // No input provided, apply gradual deceleration to stop the player
            Vector3 deceleration = -transform.position.normalized * decelerationSpeed * Time.deltaTime;
            transform.position += deceleration;

            // Check if the velocity is very small and set it to zero to avoid continuous drift
            if (transform.position.magnitude < 0.01f)
            {
                transform.position = Vector3.zero;
            }
        }
    }

    

    #region Sprint

    internal void CheckForSprint()
    {
        if (Input.GetButtonDown("Sprint") && !isSprinting)
        {
            if (Stamina.UseStamina(1))
            {
                isSprinting = true;
                stateManager.ActionState = ActionState.Sprint;
            }
            else
            {
                isSprinting = false;
                stateManager.ActionState = ActionState.Idle;
            }

        }

        if (Input.GetButtonUp("Sprint") && isSprinting) { isSprinting = false; }

        if (isSprinting)
        {
            if (Stamina.UseStamina(.25f))
            {
                isSprinting = true;
            }
            else { isSprinting = false; }
        }
        else
        {
            if (Stamina.GetCurrentStamina() <= 0)
            {
                transform.position = transform.position;
            }

        }
    }

    #endregion

    #region Dodge
    internal void CheckForDodge()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (Stamina.UseStamina(25))
            {
                Dodge();
            }
        }
        else { stateManager.ActionState = ActionState.Idle; }
    }
    
    //maybe do a raycast, check hit result, if hits something move to the hit location.
    //make distance of the raycast be dodge distance, so if nothing hti move to end of raycast
    internal void Dodge()
    {
        stateManager.ActionState = ActionState.Dodge;

        // Calculate the dodge direction based on the player's current rotation
        Vector3 dodgeDirection = transform.forward; // Move forward in the player's facing direction

        // Calculate the number of steps based on the duration
        int numSteps = Mathf.FloorToInt(dodgeDuration / Time.fixedDeltaTime);

        // Calculate the dodge step
        Vector3 dodgeStep = dodgeDirection * dodgeDistance / numSteps;

        // Start the dodge coroutine
        StartCoroutine(PerformDodge(dodgeStep, numSteps));
    }

    private IEnumerator PerformDodge(Vector3 dodgeStep, int numSteps)
    {
        for (int i = 0; i < numSteps; i++)
        {
            // Update the player's position
            transform.position += dodgeStep;

            // Wait for the next fixed update frame
            yield return new WaitForFixedUpdate();
        }
    }

    #endregion

    internal void RecoverStamina()
    {
        if (!isSprinting)
        {
            Stamina.RecoverStamina();
        }
    }

    #region Collision Damage & Death

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
    #endregion

    #region Mouse

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
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(targPos, 0.1f);
        Debug.DrawLine(transform.position, targPos, Color.cyan);
    }

}
