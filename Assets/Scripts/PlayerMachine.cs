using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SuperCharacterController))]
[RequireComponent(typeof(PlayerInputController))]
public class PlayerMachine : SuperStateMachine {

    enum PlayerStates { Idle, Walk, Jump, Fall, Sticky, NoControl }
    
    //Public reference variables. These should be updated to private sometime in the future
    public Transform AnimatedMesh;
    public Transform camera;

    //All public variables used for testing
    public float WalkSpeed = 4.0f;
    public float WalkAcceleration = 30.0f;
    public float JumpAcceleration = 5.0f;
    public float JumpHeight = 3.0f;
    public float Gravity = 25.0f;
    public float Friction = 10.0f;
    public float Slowdown = 0.1f;
    public float MaxFallSpeed = 4.0f;
    public float Glide = 1.0f;
    public bool EnableGlidey = true;
    public bool EnableSticky = true;
    public bool EnableHoppy = true;
    public bool EnableSwitching = false;
    public bool InControl = false;

    public Animator anim;  //Dee: animator

    //Private variables for different behaviours
    private float jumptime = 0;
    private bool CanDoubleJump = true;
    private RaycastHit StickWall;
    private Vector3 Lastmovedirection;
    private Vector3 moveDirection;

    //I have no idea what exactly lookDirection does ?_?
    public Vector3 lookDirection { get; private set; }
    
    //References to other scripts on this object
    private SuperCharacterController controller;
    private PlayerInputController input;

    //Debug test variables
    public Transform leftSwitchTarget;
    public Transform rightSwitchTarget;
    public Transform controlTarget;

    //Start and global updates
	void Start () {
	    // Put any code here you want to run ONCE, when the object is initialized
        input = gameObject.GetComponent<PlayerInputController>();

        anim = GetComponentInChildren<Animator>();        //Dee: INITIALIZE ANIMATOR

        // Grab the controller object from our object
        controller = gameObject.GetComponent<SuperCharacterController>();
		
		// Our character's current facing direction, planar to the ground
        lookDirection = transform.forward;

        // Set our currentState to idle on startup
        if(InControl)
        {
            currentState = PlayerStates.Idle;
            EventSystem.ActivateSwitchCharacter(controller.transform);
        }
        else
        {
            currentState = PlayerStates.NoControl;
        }
        
        //Start out lookingforward
        Lastmovedirection = lookDirection;
	}
    void OnEnable()
    {
        EventSystem.onswitchcharacter += SwitchTarget;
    }
    void OnDisable()
    {
        EventSystem.onswitchcharacter -= SwitchTarget;
    }
    protected override void EarlyGlobalSuperUpdate()
    {
        // Put any code in here you want to run BEFORE the state's update function.
        // This is run regardless of what state you're in
    }
    protected override void LateGlobalSuperUpdate()
    {
        // Put any code in here you want to run AFTER the state's update function.
        // This is run regardless of what state you're in

        // Move the player by our velocity every frame
        transform.position += moveDirection * Time.deltaTime;

        //Simoncode
        //Always save the last moved direction for when we need to stand still and look the correct way
        if(moveDirection.x != 0 || moveDirection.z != 0)
        {
            Lastmovedirection = moveDirection;
            Lastmovedirection.y = 0;
        }

        //Simoncode
        //Debug button
        if (input.Current.Debug)
        {
            float terp = Vector3.Angle(new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            int derp = 42;
        }
    }

    //Idle State
    void Idle_EnterState()
    {
        //Dee: ANIMATE
        anim.SetBool("IsWalking", false);

        controller.EnableSlopeLimit();
        controller.EnableClamping();
    }
    void Idle_SuperUpdate()
    {
        //Simoncode
        //Check if we're gonna jump
        if (input.Current.JumpInput)
        {
            currentState = PlayerStates.Jump;
            return;
        }

        //Simoncode
        //Check if we're gonna fall
        if (!MaintainingGround())
        {
            currentState = PlayerStates.Fall;
            return;
        }

        //Simoncode
        //Check if we're gonna move
        if (input.Current.MoveInput.magnitude > 0.1f)
        {
            currentState = PlayerStates.Walk;
            return;
        }

        //Apply friction to slow us to a halt
        moveDirection = Vector3.MoveTowards(moveDirection, Vector3.zero, Friction * Time.deltaTime);

        //Change our rotation to first angle ourself to the ground normal and then look in our last moved direction
        AnimatedMesh.rotation = Quaternion.FromToRotation(controller.up, controller.currentGround.PrimaryNormal());
        AnimatedMesh.rotation = AnimatedMesh.rotation * Quaternion.LookRotation(Lastmovedirection, controller.up);

        HandleSwitching();
    }

    //Walk State
    void Walk_SuperUpdate()
    {
        //Check if we're gonna jump
        if (input.Current.JumpInput)
        {
            currentState = PlayerStates.Jump;
            return;
        }

        //Check if we're gonna fall
        if (!MaintainingGround())
        {
            currentState = PlayerStates.Fall;
            return;
        }

        //Dee: ANIMATE!
        anim.SetBool("IsWalking", true);


        //Calculate movement
        if (input.Current.MoveInput != Vector3.zero)
        {
            //I NEED TO CHECK THIS CODE
            if(input.moveinput.magnitude > 0.2)
            {
                moveDirection = Vector3.MoveTowards(moveDirection, LocalMovement() * WalkSpeed * input.moveinput.magnitude , WalkAcceleration * Time.deltaTime);
            }

            //Change our rotation to first angle ourself to the ground normal and then look in our moving direction
            AnimatedMesh.rotation = Quaternion.FromToRotation(controller.up, controller.currentGround.PrimaryNormal());
            AnimatedMesh.rotation = AnimatedMesh.rotation * Quaternion.LookRotation(moveDirection, controller.up);
        }
        else
        {
            //If we're standing still, we go over to the idle state
            currentState = PlayerStates.Idle;
            return;
        }
    }

    //Jump State
    void Jump_EnterState()
    {
        //Dee: ANIMATE!
        anim.SetBool("IsJumping", true);
        anim.SetBool("HasLanded", false);


        controller.DisableClamping();
        controller.DisableSlopeLimit();

        //Give us vertical movement
        Jump(JumpHeight, Gravity);
    }
    void Jump_SuperUpdate()
    {
        //Handle both double jump and stick to walls
        HandleHoppy();
        if (HandleSticky())
            return;

        //This is unneccesary and should be rewritten :p :p :p
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

        //Handle gliding and change the vertical movement appropriately
        verticalMoveDirection = HandleGlidey(verticalMoveDirection);

        moveDirection = planarMoveDirection + verticalMoveDirection;

        //Change where we're looking to our movement, but ignore the y direction so we're stable.
        Vector3 tempdirection = moveDirection;
        tempdirection.y = 0;
        if(tempdirection != Vector3.zero)
        { 
            AnimatedMesh.rotation = Quaternion.LookRotation(tempdirection, controller.up);
        }


    }
    void Jump_ExitState()
    {
        //Dee: ANIMATE!
        anim.SetBool("IsJumping", false);
        anim.SetBool("IsDoubleJumping", false);
        anim.SetBool("HasLanded", true);

        CanDoubleJump = true;
    }

    //Fall State
    void Fall_EnterState()
    {
        controller.DisableClamping();
        controller.DisableSlopeLimit();

        //Resets our window of jump opportunity
        jumptime = 0;
    }
    void Fall_SuperUpdate()
    {
        //--WARNING--WARNING--
        //Let's just ignore this for now.
        //We should really rewrite this when we have the time :P
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

    //Sticky State
    void Sticky_EnterState()
    {
        //When we start to stick we first wanna stop all movement
        moveDirection = Vector3.zero;

        //We angle ourselves towards the wall and look upwards
        AnimatedMesh.rotation = Quaternion.FromToRotation(controller.up, StickWall.normal);
        AnimatedMesh.rotation = AnimatedMesh.rotation * Quaternion.LookRotation(Lastmovedirection, controller.up);

        //We push the slime towards the wall so it actually looks like we're sticking
        AnimatedMesh.position -= StickWall.normal * StickWall.distance;
        int derp = 0;
    }
    void Sticky_SuperUpdate()
    {
        //All we do in this state is checking if we should leave it
        if(input.Current.Sticky)
        {
            currentState = PlayerStates.Jump;
        }
    }
    void Sticky_ExitState()
    {
        //When we leave the state, we leave with momemtum away from the wall
        moveDirection += StickWall.normal * 10;
        AnimatedMesh.position = transform.position;

        //And we normalize our rotation
        AnimatedMesh.rotation = Quaternion.LookRotation(moveDirection);

    }

    //No Control State
    void NoControl_EnterState()
    {

    }
    void NoControl_SuperUpdate()
    {
        if(InControl)
        {
            currentState = PlayerStates.Idle;
        }
        Vector3 rotatetowardscharacter = controlTarget.transform.position - controller.transform.position;
        rotatetowardscharacter.y = 0;
        AnimatedMesh.rotation = Quaternion.RotateTowards(AnimatedMesh.rotation, Quaternion.LookRotation(rotatetowardscharacter), 1);
    }
    void NoControl_ExitState()
    {

    }

    //Private function used in this script
    private bool AcquiringGround()
    {
        return controller.currentGround.IsGrounded(false, 0.01f);
    }
    private bool MaintainingGround()
    {
        return controller.currentGround.IsGrounded(true, 0.5f);
    }
    private Vector3 LocalMovement()
    {
        /// <summary>
        /// Constructs a vector representing our movement local to our lookDirection, which is
        /// controlled by the camera
        /// </summary>
        Vector3 local = Vector3.zero;

        Vector3 cameraForward = Vector3.Scale(camera.forward, new Vector3(1, 0, 1)).normalized;
        local = input.Current.MoveInput.z * cameraForward + input.Current.MoveInput.x * camera.right;

        return local.normalized;
    }
    private float CalculateJumpSpeed(float jumpHeight, float gravity)
    {
        // Calculate the initial velocity of a jump based off gravity and desired maximum height attained
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }
    private void Jump(float height, float gravity)
    {
        moveDirection += controller.up * CalculateJumpSpeed(height, gravity);
    }


    //Private function used in this script which are executed continously
    private void HandleHoppy()
    {
        //Check jump input
        if (input.Current.JumpInput && CanDoubleJump && EnableHoppy)
        {
            //Dee: ANIMATE!
            anim.SetBool("IsDoubleJumping", true);

            CanDoubleJump = false;

            //Immediately make the player move in the input direction when the jump is executed
            moveDirection = LocalMovement() * WalkSpeed;
            Jump(JumpHeight, Gravity);
        }
    }
    private Vector3 HandleGlidey(Vector3 verticalmovement)
    {
        if (input.Current.ContinuousJumpInput && moveDirection.y < 0 && EnableGlidey)
        {
            return -Vector3.up * Glide;
        }
        return verticalmovement;
    }
    private bool HandleSticky()
    {
        //First we check the input and if we can stick
        if (input.Current.Sticky && EnableSticky)
        {
            //Then we check if there are any colliders in a sphere around you
            Collider[] colliders = Physics.OverlapSphere(controller.transform.position, 1);
            foreach (Collider col in colliders)
            {
                if (col.tag == "Environment")
                {
                    //When we find a sticky wall we raycast towards it's center to get the normal
                    Ray wallray = new Ray(controller.transform.position, col.bounds.center - controller.transform.position);
                    RaycastHit hit;

                    if (col.Raycast(wallray, out hit, Mathf.Infinity))
                    {
                        //Make a new ray with the direction of the wall's normal
                        wallray = new Ray(controller.transform.position, -hit.normal);

                        //Then we raycast towards the normal. This will be the closest point on the collider in almost every case.
                        if (col.Raycast(wallray, out hit, Mathf.Infinity) && hit.normal.y < 1)
                        {
                            StickWall = hit;
                            currentState = PlayerStates.Sticky;
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    private void HandleSwitching()
    {
        if (!EnableSwitching)
            return;

        if (input.Current.LeftBumper)
        {
            //Enable playercontroller on specified
            leftSwitchTarget.GetComponent<PlayerMachine>().InControl = true;

            //set new camera target
            camera.GetComponent<PlayerCamera>().SetTarget(leftSwitchTarget);

            EventSystem.ActivateSwitchCharacter(leftSwitchTarget);

            InControl = false;
            currentState = PlayerStates.NoControl;
        }
        else if (input.Current.RightBumper)
        {
            //Enable playercontroller on specified
            rightSwitchTarget.GetComponent<PlayerMachine>().InControl = true;

            //set new camera target
            camera.GetComponent<PlayerCamera>().SetTarget(rightSwitchTarget);

            EventSystem.ActivateSwitchCharacter(rightSwitchTarget);

            InControl = false;
            currentState = PlayerStates.NoControl;
        }

    }

    //Get functions
    public Vector3 GetMovement()
    {
        return moveDirection;
    }

    //Event functions
    private void SwitchTarget(Transform target)
    {
        controlTarget = target;
    }

    //Public functions other scripts use
    public void RotateGravity(Vector3 up)
    {
        lookDirection = Quaternion.FromToRotation(transform.up, up) * lookDirection;
    }
    public void ChangeMovement(Vector3 movement)
    {
        //Simoncode
        //Changes the movement on command. Used in bounce mushrooms.
        moveDirection = movement;
        CanDoubleJump = true;
    }
}
