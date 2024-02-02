using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using System;
using Cinemachine;
using Unity.Burst.CompilerServices;
using TMPro;

public class PlayerMovement : Player
{
   private PlayerMovement PlayerMove;


    [Header("Camera Properties")]
    [SerializeField]
    GameObject VirtualCamera;
    [SerializeField]
    float ShakeIntensity;
    [SerializeField]
    float totalShakeTime;
    float shakeTimer;

    CinemachineVirtualCamera CMVcam;

    public static event Action OnPlayerDeath;


    public StaminaScript Stamina;

    [SerializeField]
    private Animator playerAnimator;

    public float Speed = 8f;
    //private bool isSprinting = false;

    public float rotationSpeed = 360.0f;

    public float dodgeDistance = 2.5f;
    public float dodgeDuration = 0.05f;

    ////Made this public so trap can check if player can take damage, probably better way to do this
    //public bool canTakeDamage = true;

    //public float damageCooldownDuration = 1f;

    public float decelerationFactor = 10.0f; // Adjust the value as needed

    Plane plane;

    [SerializeField]
    public bool usingController;
    void Start()
    {
        PlayerMove = GetComponent<PlayerMovement>();
        //player = GetComponent<Player>();
        plane = new Plane(Vector3.down, transform.position.y);
        CMVcam = VirtualCamera.GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (GetHealth() == 0)
        {
            OnGameOver();
        }

        // Check if the player wants to sprint
        //CheckForSprint();
        CheckForHealthPickup();

        CheckForDodge();

        RecoverStamina();

        //if (PlayerStateManager.instance.ActionState != ActionState.Attack)
        //{
            
        //}
        if (usingController)
        {
            JoystickAim();
        }
        else
        {
            LookAtMouse();
        }
        UpdateShakeTimer();
    }

    private void JoystickAim()
    {
        Vector3 JoystickAimDirection = GetJoystickAimDirection();
        if(JoystickAimDirection.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(JoystickAimDirection , Vector3.up);
        }
    }

    public Vector3 GetJoystickAimDirection()
    {
        return getCameraRight() * Input.GetAxisRaw("RSHorizontal") + getCameraForward() * Input.GetAxisRaw("RSVertical");
    }

    private void FixedUpdate()
    {
        //if(PlayerStateManager.instance.ActionState != ActionState.Attack)
        //{
            
        //}
        Movement();

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

    public void Movement()
    {
        // Movement
        float currentSpeed = Speed;
        

        // Calculate movement direction based on camera's perspective
        Vector3 cameraForward = getCameraForward();
        Vector3 cameraRight = getCameraRight();

        //get move vector from different axis mapping depending on if using controller or not
        Vector3 move = getMoveVector(cameraForward, cameraRight);
        playerAnimator.SetFloat("speed", move.magnitude);

        Vector3 newPosition = transform.position + move * currentSpeed * Time.deltaTime;
        transform.position = newPosition;

    }

    private Vector3 getMoveVector(Vector3 cameraForward, Vector3 cameraRight)
    {
        Vector3 move = Vector3.zero;
        if (usingController)
        {
            move = (Input.GetAxisRaw("LSVertical") * cameraForward + Input.GetAxisRaw("LSHorizontal") * cameraRight).normalized;
        }
        else
        {
            // Calculate the movement direction based on input and camera orientation
            move = (Input.GetAxisRaw("Vertical") * cameraForward + Input.GetAxisRaw("Horizontal") * cameraRight).normalized;
        }
        return move;
    }

    #region Dodge
    internal void CheckForDodge()
    {
        if (Input.GetButtonDown("Dodge"))
        {
            Debug.Log("Player hit: Dodge | left shift key");
            if (Stamina.UseStamina(25))
            {
                Dodge();
            }
        }
        //else { stateManager.ActionState = ActionState.Idle; }
    }
    
    //maybe do a raycast, check hit result, if hits something move to the hit location.
    //make distance of the raycast be dodge distance, so if nothing hti move to end of raycast
    internal void Dodge()
    {
        //stateManager.ActionState = ActionState.Dodge;

        //Vector3 dodgeDirection = transform.forward;

        // Get input from WASD keys
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Construct dodge direction from input
        Vector3 dodgeDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        RaycastHit hit;
        Ray dodgeRay = new Ray(transform.position, dodgeDirection);
        Debug.DrawRay(transform.position, dodgeDirection, Color.cyan, dodgeDuration);
        if (Physics.Raycast(dodgeRay, out hit, dodgeDistance))
        {
            float distance = hit.distance;

            // Calculate the dodge direction based on the player's current rotation
            // Move forward in the player's facing direction

            // Calculate the number of steps based on the duration
            int numSteps = Mathf.FloorToInt(dodgeDuration / Time.fixedDeltaTime);

            // Calculate the dodge step 
            //.2 is magic number, could get meshrender.bounds.extents.x, bassicly here to give a bit of buffer so we dont end up halfway inside things
            Vector3 dodgeStep = dodgeDirection * (distance-0.2f) / numSteps;

            // Start the dodge coroutine
            StartCoroutine(PerformDodge(dodgeStep, numSteps));
        }
        else
        {
            // Calculate the number of steps based on the duration
            int numSteps = Mathf.FloorToInt(dodgeDuration / Time.fixedDeltaTime);

            // Calculate the dodge step
            Vector3 dodgeStep = dodgeDirection * dodgeDistance / numSteps;

            // Start the dodge coroutine
            StartCoroutine(PerformDodge(dodgeStep, numSteps));
        }
    }

    private IEnumerator PerformDodge(Vector3 dodgeStep, int numSteps)
    {
        for (int i = 0; i < numSteps; i++)
        {
           
           transform.position += dodgeStep;
            // Wait for the next fixed update frame
            yield return new WaitForFixedUpdate();
        }
    }

    #endregion

    #region Health Pickup
    internal void CheckForHealthPickup()
    {
        if (Input.GetButtonDown("UseHealth"))
        {
            Debug.Log("Player hit: UseHealth | E key");
            UseHealth();
        }
    }
    #endregion

    internal void RecoverStamina()
    {
        Stamina.RecoverStamina();
    }

    #region Collision Damage & Death

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            if (canTakeDamage)
            {
                PlayerHasBeenHit();
            }

        }
        else if (collision.gameObject.CompareTag("Trap"))
        {
            if (canTakeDamage)
            {
                PlayerHasBeenHit();
            }
        }
    }

    //made this so other damaging classes can acesss, need feedback if this is fine - Marco
    public void PlayerHasBeenHit()
    {
        canTakeDamage = false;
        StartCoroutine(DamageCooldown());
        // Reduce player's health when colliding with an enemy
        ManageDamage(1);
    }

    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(damageCooldownDuration);
        canTakeDamage = true; // Allow taking damage again after cooldown
    }

    //Made take damage public so trap could access it, may also need to be refactored
    void ManageDamage(int damage)
    {
        if(GetHealth() > 0)
        {
            TakeDamage(damage);

            MusicManager.instance.SetLowPassCutoffBasedOnHealth((float)GetHealth() / (float)GetMaxHealth());
            canTakeDamage = true;
            ShakeCamera();
            // Check for player death
            if (GetHealth() == 0)
            {
                StopCameraShake();
                OnGameOver();
            }

        }
    }

    public void OnGameOver()
    {
        //set something on enemymanager
        EnemyManager.Instance.StopTimer();
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

    public void DisablePlayer() // for when the player dies so they don't keep moving around
    {
        PlayerMove.enabled = false;
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

    #region Camera

    public void ShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin noise = CMVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = ShakeIntensity;
        noise.m_FrequencyGain = 5;
        shakeTimer = totalShakeTime;
    }

    public void StopCameraShake()
    {
        CinemachineBasicMultiChannelPerlin noise = CMVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }

    void UpdateShakeTimer()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                StopCameraShake();
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
