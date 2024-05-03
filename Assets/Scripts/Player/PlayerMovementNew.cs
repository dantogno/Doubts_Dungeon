using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerMovementNew : MonoBehaviour
{
    // P R O P E R T I E S

    [Header("Movement Settings")]
    [SerializeField] Player player;
    public float Speed = 8f; //Needed for DamageSurface
    [HideInInspector]
    public float DefaultSpeed;
    //[SerializeField] float interpolationFactor = 1f;

    [SerializeField] float rotationSpeed = 360.0f;
    [SerializeField] float dodgeDistance = 2.5f;
    [SerializeField] float dodgeDuration = 0.05f;
    //[SerializeField] float decelerationFactor = 10.0f; // Adjust the value as needed
    public StaminaScript Stamina;


    public bool NewMovmentTest = false;

    // INPUT ACTION STUFF
    Vector2 currentMoveInput;
    Vector3 moveVector;
    

    [Header("Animation Settings")]
    [SerializeField] Animator playerAnimator;


    [Header("Misc")]
    Plane plane;

    [SerializeField]
    public InputType PlayerInputMode;

    [Header("References")]
    public ShootBehavior SB;
    private Rigidbody rb;
    CinemachineVirtualCamera CMVcam;
    [SerializeField]
    //OnScreenConsole console;
    Vector3 SpawnPosition;


    // U N I T Y  M E T H O D S

    private void Awake()
    {
        DefaultSpeed = Speed;

        playerAnimator = GetComponent<Animator>();
        SB = GetComponent<ShootBehavior>();
        rb = GetComponent<Rigidbody>();

    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        Speed = player.Speed;

        plane = new Plane(Vector3.down, transform.position.y);
        GameObject spawnLocation = GameObject.Find("SpawnLocation");
        SpawnPosition = spawnLocation.transform.position;

        gameObject.transform.position = SpawnPosition;
    }

    private void OnLevelWasLoaded(int level)
    {
        GameObject CameraPivot = GameObject.Find("Camera Pivot");
        Transform VirtualCamera = CameraPivot.transform.Find("Virtual Camera");
        CMVcam = VirtualCamera.GetComponent<CinemachineVirtualCamera>();

        CMVcam.Follow = transform;


        //Transform gameoverScreen = canvas.transform.Find("GameOverScreen");
        //Transform highscoreTransform = gameoverScreen.transform.Find("ScoreText");
        //HighscoreText = highscoreTransform.GetComponent<TextMeshProUGUI>();

        GameObject spawnLocation = GameObject.Find("SpawnLocation");
        SpawnPosition = spawnLocation.transform.position;

        gameObject.transform.position = SpawnPosition;
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckCameraReference();
    }

    private void FixedUpdate()
    {
        //Move();
        CheckForDodge();
        HandleLookDirection();
    }



    #region 'Mouse Look'
    // L O O K  D I R E C T I O N
    void HandleLookDirection()
    {
        switch(PlayerInputMode)
        {
            case InputType.Keyboard:
                LookAtMouse();
                break;

            case InputType.Controller:

                break;
        }
    }
    void LookAtMouse()
    {
        // turns the player to be looking at the mouse

        // converting mouse pos to a ray
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

    void UpdateLookRotation(float vertInput, float horizInput)
    {
        Vector3 input = new Vector3(vertInput , 0 , -horizInput );
        transform.LookAt(transform.position + input);
        //rb.rotation = Quaternion.identity;
        rb.angularVelocity = Vector3.zero;
    }

    #endregion

    // M O V E M E N T 
    
    void Move()
    {
        //update speed
        Speed = player.Speed;

        // moves the player 
        float vertInput = Input.GetAxisRaw("Vertical");
        float horizInput = Input.GetAxisRaw("Horizontal");

        Vector3 move = GetMoveVector(vertInput, horizInput);
        UpdateLookRotation(vertInput, horizInput);
        playerAnimator.SetFloat("speed", move.magnitude);

        // Adjust movement speed
        move *= Speed * Time.deltaTime;

        Vector3 movePosition = transform.position + move;

        transform.position = movePosition;


        if (NewMovmentTest)
        {
            //Animating
            float velocityZ = Vector3.Dot(move.normalized, transform.forward);
            float velocityX = Vector3.Dot(move.normalized, transform.right);

            playerAnimator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
            playerAnimator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);
        }
    }

    Vector3 GetMoveVector(float vertInput, float horizInput)
    {
        //calculates the move vector for the player 

        Vector3 move = Vector3.zero;

        switch (PlayerInputMode)
        {
            case InputType.Keyboard:
                move = (vertInput * getCameraForward()
                    + horizInput * getCameraRight()).normalized;
                break;

            case InputType.Controller:
                move = (Input.GetAxis("LSVertical") * getCameraForward()
                    + Input.GetAxis("LSHorizontal") * getCameraRight()).normalized;
                break;
        }
        //console.Log("Move Vector", move, 0);
        return move;
        
    }
    Vector3 getCameraForward()
    {
        // Calculate movement direction based on camera's perspective
        Vector3 cameraForward = Camera.main.transform.forward;//Breaking
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


    #region 'Dodge'

    void Dodge()
    {
        // Get input from WASD keys
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Construct dodge direction from input
        Vector3 dodgeDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        //raycast for dodge distatnce
        RaycastHit hit;
        Ray dodgeRay = new Ray(transform.position, dodgeDirection);
        //draws the ray for debug
        Debug.DrawRay(transform.position, dodgeDirection, Color.cyan, dodgeDuration);
        
        // if we hit something, the distance from us to the thing is the distance we dodge
        if (Physics.Raycast(dodgeRay, out hit, dodgeDistance))
        {
            float distance = hit.distance;

            // Calculate the dodge direction based on the player's current rotation
            // Move forward in the player's facing direction

            // Calculate the number of steps based on the duration
            int numSteps = Mathf.FloorToInt(dodgeDuration / Time.fixedDeltaTime);

            // Calculate the dodge step 
            //.2 is magic number, could get meshrender.bounds.extents.x,
            //      bassicly here to give a bit of buffer so we dont end up halfway inside things
            Vector3 dodgeStep = dodgeDirection * (distance - 0.2f) / numSteps;

            // Start the dodge coroutine
            StartCoroutine(PerformDodge(dodgeStep, numSteps));
        }
        // if we don't hit something, dodge the set dodge distance?
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

    void CheckForDodge()
    {
        if (Input.GetButtonDown("Dodge"))
        {
            Debug.Log("Player hit: Dodge | left shift key");
            if (Stamina.UseStamina(25))
            {
                Dodge();
            }
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

    public void DisablePlayer()
    {
        enabled = false;
    }

    void CheckCameraReference()
    {
        //if (CMVcam == null)
        //{
        //    CMVcam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();//break
        //}
    }
}
