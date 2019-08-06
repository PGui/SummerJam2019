using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalCoop;

public class PlayerController : MonoBehaviour
{
    public bool debugMode = false;
    [ReadOnly] public int playerControllerID = 0;
    [ReadOnly] public bool gameplayControlsEnabled = false;
   enum State {Idle, Running, Dashing, Jumping};
    private State state = State.Idle;
    private CharacterController controller;
    private Vector3 moveDirection;
    private string horizontalInput = "Horizontal";
    private string verticalInput = "Vertical";
    private string dashInput = "Fire1";
    private string jumpInput = "Jump";
    public const float maxDashTime = 1.0f;
    private float currentDashTime = maxDashTime;
    private bool isInAir = false;
    private bool canDash = true;

    private bool isJumping = false;

    private float jumpTimeHeld = 0.0f;
    public float jumpMaxTime = 1.0f;
    
    public float movementSpeed = 0;
    public float directionThreshold = 0.2f;
    public float groundSpeed = 10;
    public float airSpeed = 5.0f;
    public float movementDamping = 0.1f;
    public float dashSpeed = 100;
    public bool momentum = false;
    public float dashDistance = 10;
    public Vector3 drag = new Vector3(2.0f, 2.0f, 2.0f);

    public float minJumpHeight = 1.0f;
    public float maxJumpHeight = 5.0f;
    public float jumpMultiplier = -2.0f;
    public float gravityMultiplier = 2.0f;

    private Vector3 velocity;

    private Vector3 previousMove;
    private Vector3 moveVelocity;

    private float lastTimeGrounded = 0.0f;
    private bool wasGrounded = false;
    public float jumpFall = 0.15f;
    private bool isFalling = false;

    private Vector3 lastDirection;

       // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        moveDirection = Vector3.zero;

        StartCoroutine(CheckGrounded(controller));
    }

    // Update is called once per frame
    void Update()
    {
        if(!debugMode && !gameplayControlsEnabled)
            return;

        string input_horizontal = debugMode ? "Horizontal_P2": PlayerManager.K_HORIZONTAL+playerControllerID.ToString();
        string input_vertical = debugMode ? "Vertical_P2": PlayerManager.K_VERTICAL+playerControllerID.ToString();
        Vector3 move = new Vector3(Input.GetAxis(input_horizontal), 0, Input.GetAxis(input_vertical));
        if (move != Vector3.zero)
        {
            transform.forward = move;
        }

        Vector3 moveVector = move * GetSpeedToApply(move);

        Vector3 smoothedVector = Vector3.SmoothDamp(previousMove, moveVector, ref moveVelocity, movementDamping);

        string input_dash = debugMode ? "Dash_P2": PlayerManager.K_DASH+playerControllerID.ToString();
        if (Input.GetButtonDown(input_dash) && currentDashTime > maxDashTime)
        {
            currentDashTime = 0.0f;
            smoothedVector += Vector3.Scale(transform.forward,
                                            dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * drag.x + 1)) / -Time.deltaTime), 
                                                                        0, 
                                                                    (Mathf.Log(1f / (Time.deltaTime * drag.z + 1)) / -Time.deltaTime)));
        }

        controller.Move(smoothedVector * Time.deltaTime);

        previousMove = smoothedVector;
        
        velocity.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = 0f;
        
        string input_jump = debugMode ? "Jump_P2": PlayerManager.K_JUMP+playerControllerID.ToString();
        if (Input.GetButtonDown(input_jump) && (controller.isGrounded || 
            (isFalling && lastTimeGrounded < jumpFall)))
        {
            lastDirection = transform.forward;
            isJumping = true;
             velocity.y = 0f;
             velocity.y += Mathf.Sqrt(maxJumpHeight * jumpMultiplier * Physics.gravity.y);
        }

        currentDashTime += Time.deltaTime;
    }

    float GetSpeedToApply(Vector3 axisDirection)
    {
        float Speed = groundSpeed;

        if (!controller.isGrounded)
        {
            axisDirection.Normalize();
            lastDirection.Normalize();
            float delta = (Vector3.Dot(axisDirection, lastDirection) + 1.0f) / 2.0f;

            Speed = Mathf.Lerp(airSpeed, groundSpeed, delta);
        }

        return Speed;
    }

    IEnumerator CheckGrounded(CharacterController controller)
    {
        while(true)
        {
            if (!controller.isGrounded)
            {
                lastTimeGrounded += Time.deltaTime;
            }
            else
            {
                lastTimeGrounded = 0.0f;
            }

            if (controller.isGrounded && !wasGrounded)
            {
                if (isJumping)
                    isJumping = false;
            }

            if (!isJumping && !controller.isGrounded)
            {
                isFalling = true;
            }
            else if (isFalling && controller.isGrounded)
            {
                isFalling = false;
            }

            wasGrounded = controller.isGrounded;
            
            yield return null;
        }
    }

    

    void Land()
    {
        isInAir = false;
    }

    void Dash()
    {
        moveDirection = Vector3.zero;
        state = State.Dashing;
        currentDashTime = 0;    
        Vector3 direction= new Vector3(Input.GetAxis(horizontalInput), 0, Input.GetAxis(verticalInput));
        if (direction.sqrMagnitude < directionThreshold)  transform.LookAt(transform.position + direction);
        moveDirection = transform.forward * dashDistance;
        movementSpeed = dashSpeed;
       
     
    }
    void Run(float speed)
    {
        Vector3 direction = new Vector3(Input.GetAxis(horizontalInput), 0, Input.GetAxis(verticalInput));
		    
        if (direction.sqrMagnitude > directionThreshold)
        {
            transform.LookAt(transform.position + direction);
             moveDirection += transform.forward *  direction.magnitude;
            movementSpeed = speed;
        }
    }

    void EndDash()
    {
        if(!isInAir){
            canDash = true;
            movementSpeed = groundSpeed;
            moveDirection = Vector3.zero;
            state = State.Idle;
        } 
    }
}