using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerController : MonoBehaviour
{
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
        if (Input.GetButtonUp("Jump"))
        {
            jumpTimeHeld = 0.0f;
        }

        if (Input.GetButton("Jump"))
        {
            jumpTimeHeld += Time.deltaTime;
            jumpTimeHeld = Mathf.Min(jumpTimeHeld, jumpMaxTime);
        } 

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (move != Vector3.zero)
        {
            transform.forward = move;
        }

        Vector3 moveVector = move * (controller.isGrounded ? groundSpeed : airSpeed);

        Vector3 smoothedVector = Vector3.SmoothDamp(previousMove, moveVector, ref moveVelocity, movementDamping);

        controller.Move(smoothedVector* Time.deltaTime);

        previousMove = smoothedVector;
        
        velocity.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = 0f;
        
        if (Input.GetButtonDown("Jump") && (controller.isGrounded || 
            (isFalling && lastTimeGrounded < jumpFall)))
        {
            isJumping = true;
             velocity.y = 0f;
             velocity.y += Mathf.Sqrt(maxJumpHeight * jumpMultiplier * Physics.gravity.y);
        }

        if (false/* Input.GetButtonDown("Dash")*/)
        {
            Debug.Log("Dash");
            velocity += Vector3.Scale(transform.forward, 
                                       dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * drag.x + 1)) / -Time.deltaTime), 
                                                                  0, 
                                                                  (Mathf.Log(1f / (Time.deltaTime * drag.z + 1)) / -Time.deltaTime)));
        }


        // velocity.x /= 1 + drag.x * Time.deltaTime;
        // velocity.y /= 1 + drag.y * Time.deltaTime;
        // velocity.z /= 1 + drag.z * Time.deltaTime;
        
        // if(Input.GetButtonUp(dashInput) && canDash)
        // {
        //    Dash(); 
        // } 

        // if( state == State.Dashing)
        // {
        //     if(currentDashTime < maxDashTime)
        //     {
        //        currentDashTime += dashStoppingSpeed;
        //     }
        //     else
        //     {
        //         EndDash();
        //     }
        // }
        // else if(state == State.Running || state == State.Idle)
        // {
        //     Run(groundSpeed); 
        //     if(moveDirection.magnitude > directionThreshold)
        //     {
        //         state = State.Running;
        //      } 
        //     else
        //     {
        //         state = State.Idle;
        //         movementSpeed = 0;
        //     } 
        // } 
       
        // controller.Move(moveDirection * movementSpeed * Time.deltaTime );
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