﻿using UnityEngine;
using System.Collections;

/*
 * Example implementation of the SuperStateMachine and SuperCharacterController
 */
[RequireComponent(typeof(SuperCharacterController))]
[RequireComponent(typeof(PlayerInputController))]
public class PlayerMachine : SuperStateMachine {

    public Transform AnimatedMesh;

    public float WalkSpeed = 4.0f;
    public float WalkAcceleration = 30.0f;
    public float JumpAcceleration = 5.0f;
    public float JumpHeight = 3.0f;
    public float Gravity = 25.0f;
    public float Friction = 10.0f;
    public float Slowdown = 0.1f;
    public float MaxFallSpeed = 4.0f;
    public float Glide = 1.0f;
    private float jumptime = 0;
    private bool CanDoubleJump = false;

    // Add more states by comma separating them
    enum PlayerStates { Idle, Walk, Jump, Fall }

    private SuperCharacterController controller;

    // current velocity
    private Vector3 moveDirection;
    // current direction our character's art is facing
    public Vector3 lookDirection { get; private set; }

    public Transform camera;

    private PlayerInputController input;

	void Start () {
	    // Put any code here you want to run ONCE, when the object is initialized

        input = gameObject.GetComponent<PlayerInputController>();

        // Grab the controller object from our object
        controller = gameObject.GetComponent<SuperCharacterController>();
		
		// Our character's current facing direction, planar to the ground
        lookDirection = transform.forward;

        // Set our currentState to idle on startup
        currentState = PlayerStates.Idle;
	}

    protected override void EarlyGlobalSuperUpdate()
    {
		// Rotate out facing direction horizontally based on mouse input
        //lookDirection = Quaternion.AngleAxis(input.Current.MouseInput.x, controller.up) * lookDirection;
        // Put any code in here you want to run BEFORE the state's update function.
        // This is run regardless of what state you're in
    }

    protected override void LateGlobalSuperUpdate()
    {
        // Put any code in here you want to run AFTER the state's update function.
        // This is run regardless of what state you're in

        // Move the player by our velocity every frame
        transform.position += moveDirection * Time.deltaTime;



        //Vector3 temp = AnimatedMesh.eulerAngles;
        //temp.x = 0;
        //AnimatedMesh.eulerAngles = temp;
    }

    private bool AcquiringGround()
    {
        return controller.currentGround.IsGrounded(false, 0.01f);
    }

    private bool MaintainingGround()
    {
        return controller.currentGround.IsGrounded(true, 0.5f);
    }

    public void RotateGravity(Vector3 up)
    {
        lookDirection = Quaternion.FromToRotation(transform.up, up) * lookDirection;
    }

    //Simoncode
    //Changes the movement on command. Used in bounce mushrooms.
    public void ChangeMovement(Vector3 movement)
    {
        //currentState = PlayerStates.Jump;
        moveDirection = movement;
        CanDoubleJump = true;
    }
    /// <summary>
    /// Constructs a vector representing our movement local to our lookDirection, which is
    /// controlled by the camera
    /// </summary>
    private Vector3 LocalMovement()
    {
        //Vector3 right = Vector3.Cross(controller.up, lookDirection);

        Vector3 local = Vector3.zero;

        //if (input.Current.MoveInput.x != 0)
        //{
        //    local += right * input.Current.MoveInput.x;
        //}

        //if (input.Current.MoveInput.z != 0)
        //{
        //    local += lookDirection * input.Current.MoveInput.z;
        //}
            Vector3 cameraForward = Vector3.Scale(camera.forward, new Vector3(1, 0, 1)).normalized;
            local = input.Current.MoveInput.z * cameraForward + input.Current.MoveInput.x * camera.right;

        return local.normalized;
    }

    // Calculate the initial velocity of a jump based off gravity and desired maximum height attained
    private float CalculateJumpSpeed(float jumpHeight, float gravity)
    {
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

	/*void Update () {
	 * Update is normally run once on every frame update. We won't be using it
     * in this case, since the SuperCharacterController component sends a callback Update 
     * called SuperUpdate. SuperUpdate is recieved by the SuperStateMachine, and then fires
     * further callbacks depending on the state
	}*/

    // Below are the three state functions. Each one is called based on the name of the state,
    // so when currentState = Idle, we call Idle_EnterState. If currentState = Jump, we call
    // Jump_SuperUpdate()
    void Idle_EnterState()
    {
        controller.EnableSlopeLimit();
        controller.EnableClamping();
    }

    void Idle_SuperUpdate()
    {
        // Run every frame we are in the idle state

        if (input.Current.JumpInput)
        {
            currentState = PlayerStates.Jump;
            return;
        }

        if (!MaintainingGround())
        {
            currentState = PlayerStates.Fall;
            return;
        }

        if (input.Current.MoveInput != Vector3.zero)
        {
            currentState = PlayerStates.Walk;
            return;
        }

        // Apply friction to slow us to a halt
        moveDirection = Vector3.MoveTowards(moveDirection, Vector3.zero, Friction * Time.deltaTime);
        AnimatedMesh.rotation = Quaternion.FromToRotation(controller.up, controller.currentGround.PrimaryNormal());
        AnimatedMesh.rotation = AnimatedMesh.rotation * Quaternion.LookRotation(moveDirection, controller.up);

    }

    void Idle_ExitState()
    {
        // Run once when we exit the idle state
    }

    void Walk_SuperUpdate()
    {
        if (input.Current.JumpInput)
        {
            currentState = PlayerStates.Jump;
            return;
        }

        if (!MaintainingGround())
        {
            currentState = PlayerStates.Fall;
            return;
        }

        if (input.Current.MoveInput != Vector3.zero)
        {
            if(input.moveinput.magnitude > 1.2)
            {
                moveDirection = Vector3.MoveTowards(moveDirection, LocalMovement() * WalkSpeed, WalkAcceleration * Time.deltaTime);
            }
            else
            {
                moveDirection = Vector3.MoveTowards(moveDirection, LocalMovement() * WalkSpeed * input.moveinput.magnitude , WalkAcceleration * Time.deltaTime);
            }
            
            // Rotate our mesh to face where we are "looking"
            //Simon: I moved this code

            AnimatedMesh.rotation = Quaternion.FromToRotation(controller.up, controller.currentGround.PrimaryNormal());
            AnimatedMesh.rotation = AnimatedMesh.rotation * Quaternion.LookRotation(moveDirection, controller.up);
            //AnimatedMesh.transform.position += controller.currentGround.PrimaryNormal() * controller.currentGround.Distance();
        }
        else
        {
            currentState = PlayerStates.Idle;
            return;
        }
    }

    void Jump_EnterState()
    {
        controller.DisableClamping();
        controller.DisableSlopeLimit();

        moveDirection += controller.up * CalculateJumpSpeed(JumpHeight, Gravity);


    }

    void Jump_SuperUpdate()
    {
        Vector3 planarMoveDirection = Math3d.ProjectVectorOnPlane(controller.up, moveDirection);
        Vector3 verticalMoveDirection = moveDirection - planarMoveDirection;

        //This calculates if we touch the ground
        if (Vector3.Angle(verticalMoveDirection, controller.up) > 90 && AcquiringGround())
        {
            moveDirection = planarMoveDirection;
            currentState = PlayerStates.Idle;
            moveDirection *= Slowdown;
            return;            
        }

        planarMoveDirection = Vector3.MoveTowards(planarMoveDirection, LocalMovement() * WalkSpeed, JumpAcceleration * Time.deltaTime);
        if(verticalMoveDirection.y > -30)
        {
            verticalMoveDirection -= controller.up * Gravity * Time.deltaTime;
            //verticalMoveDirection = Vector3.MoveTowards(verticalMoveDirection, verticalMoveDirection - controller.up * Gravity * Time.deltaTime, MaxFallSpeed * Time.deltaTime);
        }
        //Simoncode
        if (input.Current.ContinuousJumpInput && moveDirection.y < 0)
        {
            moveDirection = planarMoveDirection + -Vector3.up * Glide;
        }
        else
        {
            //This is the standard air movement
            moveDirection = planarMoveDirection + verticalMoveDirection;
        }

        Vector3 tempdirection = moveDirection;
        tempdirection.y = 0;
        if(tempdirection != Vector3.zero)
        { 
            AnimatedMesh.rotation = Quaternion.LookRotation(tempdirection, controller.up);
        }

        

        //Simoncode
        //Do the double jump
        if (input.Current.JumpInput && CanDoubleJump)
        {
            CanDoubleJump = false;
            moveDirection = LocalMovement() * WalkSpeed;
            moveDirection += controller.up * CalculateJumpSpeed(JumpHeight, Gravity);
            return;
        }

    }

    void Jump_ExitState()
    {
        CanDoubleJump = true;
    }

    void Fall_EnterState()
    {
        jumptime = 0;

        controller.DisableClamping();
        controller.DisableSlopeLimit();

        // moveDirection = trueVelocity;
    }

    void Fall_SuperUpdate()
    {
        jumptime += Time.deltaTime;
        if (jumptime < 0.1 && input.Current.JumpInput)
        {
            currentState = PlayerStates.Jump;
            return;
        }

        if (AcquiringGround())
        {
            moveDirection = Math3d.ProjectVectorOnPlane(controller.up, moveDirection);
            currentState = PlayerStates.Idle;
            return;
        }

        moveDirection -= controller.up * Gravity * Time.deltaTime;
    }
}
