using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalCoop;

public delegate void OnDashDelegate();
public delegate void OnJumpDelegate();
public delegate void OnMoveDelegate();
public delegate void OnStopDelegate();

public class PlayerController : MonoBehaviour
{
    [Header("Debug")]
    public bool debugMode = false;
    public string playerControllerID = "1";
    [ReadOnly] public bool gameplayControlsEnabled = false;
    // Player controller internals
    private CharacterController controller;
    private Rigidbody rb;

    [Header("Movement")]
    public float groundSpeed = 10;
    public float airSpeed = 5.0f;
    public float movementDamping = 0.1f;
    public float gravityMultiplier = 2.0f;
    public OnMoveDelegate OnMove;
    public OnStopDelegate OnStop;

    [Header("Jump")]
    public float jumpMultiplier = -2.0f;
    public float maxJumpHeight = 5.0f;
    public float jumpFall = 0.15f;
    public OnJumpDelegate OnJump;

    [Header("Dash")]
    public float dashDistance = 10;
    public Vector3 drag = new Vector3(2.0f, 2.0f, 2.0f);
    public OnDashDelegate OnDash;
    public const float maxDashTime = 1.0f;
    [Header("Out of Arena Bump")]
    
    public bool isMagnet = false;
    public Vector3 magnetPosition = new Vector3(3.0f,20.0f,-8.0f);
    private Vector3 magnetDirection = new Vector3(0.0f,0.0f,0.0f);
    public float magnetFactor = 3.0f;
    public float outOfArenaJumpMultiplier = 5.0f;

    [Header("Capture")]
    public const float maxCaptureTime = 0.5f;
    public bool canCapture = false;

    // Internals movement
    private Vector3 moveDirection;
    private Vector3 velocity;
    private Vector3 previousMove;
    private Vector3 moveVelocity;
    private float lastTimeGrounded = 0.0f;
    private bool wasGrounded = false;

    // Internals jump
    private bool isJumping = false;
    private bool isFalling = false;
    private Vector3 lastDirection;
    
    // Internals dash
    private float currentDashTime = maxDashTime;


 
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        moveDirection = Vector3.zero;

        StartCoroutine(CheckGrounded(controller));
    }
    // Update is called once per frame
    void Update()
    {
        if(!debugMode && !gameplayControlsEnabled)
            return;

        Vector3 move = new Vector3(Input.GetAxis(GetHorizonalInputString()), 0, Input.GetAxis(GetVerticalInputString()));
        if (move != Vector3.zero)
        {
            OnMove();
            transform.forward = move;
        }
        else
        {
            OnStop();
        }

        Vector3 moveVector = move * GetSpeedToApply(move);

        Vector3 smoothedVector = Vector3.SmoothDamp(previousMove, moveVector, ref moveVelocity, movementDamping);

        if (Input.GetButtonDown(GetDashInputString()) && currentDashTime > maxDashTime)
        {
            OnDash();
            currentDashTime = 0.0f;
            smoothedVector += Vector3.Scale(transform.forward,
                                            dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * drag.x + 1)) / -Time.deltaTime), 
                                                                        0, 
                                                                    (Mathf.Log(1f / (Time.deltaTime * drag.z + 1)) / -Time.deltaTime)));
            canCapture = true;
        }

        if(isMagnet)
        {
            smoothedVector += magnetDirection*magnetFactor;
            if(controller.isGrounded)
            {
                isMagnet = false;
            }
        }

        controller.Move(smoothedVector * Time.deltaTime);

        previousMove = smoothedVector;
        
        velocity.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = 0f;
        
        if (Input.GetButtonDown(GetJumpInputString()) && (controller.isGrounded || 
            (isFalling && lastTimeGrounded < jumpFall)))
        {
            OnJump();
            lastDirection = transform.forward;
            isJumping = true;
             velocity.y = 0f;
             velocity.y += Mathf.Sqrt(maxJumpHeight * jumpMultiplier * Physics.gravity.y);
        }

        currentDashTime += Time.deltaTime;

        if(currentDashTime >= maxCaptureTime)
            canCapture = false;
        

           

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

    public void TriggerOutOfArenaBump()
    {
            isMagnet = true;
            lastDirection = transform.forward;
            isJumping = true;
            velocity.y = 0f;
            velocity.y += Mathf.Sqrt(outOfArenaJumpMultiplier*maxJumpHeight * jumpMultiplier * Physics.gravity.y);
            currentDashTime = 0.0f;
            magnetDirection = magnetPosition - this.gameObject.transform.position;
            magnetDirection.Normalize();
    }
    string GetHorizonalInputString()
    {
        return PlayerManager.K_HORIZONTAL+playerControllerID;
    }
    string GetVerticalInputString()
    {
        return PlayerManager.K_VERTICAL+playerControllerID;
    }
    string GetJumpInputString()
    {
        return PlayerManager.K_JUMP+playerControllerID;
    }
    string GetDashInputString()
    {
        return PlayerManager.K_DASH+playerControllerID;
    }
}