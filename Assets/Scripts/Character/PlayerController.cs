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
   
    
    public float movementSpeed = 0;
    public float directionThreshold = 0.2f;
    public float groundSpeed = 10;
    public float airSpeed = 5.0f;
    public float movementDamping = 0.1f;
    public float dashSpeed = 100;
    public bool momentum = false;
    public float dashDistance = 10;
    public float dashStoppingSpeed = 0.1f;
    public float jumpHeight = 10.0f;

    private Vector3 velocity;

    private Vector3 previousMove;
    private Vector3 moveVelocity;

       // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        moveDirection = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        Vector3 moveVector = move * (controller.isGrounded ? groundSpeed : airSpeed);

        Vector3 smoothedVector = Vector3.SmoothDamp(previousMove, moveVector, ref moveVelocity, movementDamping);

        controller.Move(smoothedVector* Time.deltaTime);

        previousMove = smoothedVector;
        if (move != Vector3.zero)
        {
            transform.forward = move;
        }

        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = 0f;

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
            velocity.y += Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        
        
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