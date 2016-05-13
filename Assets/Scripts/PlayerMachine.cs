using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(SuperCharacterController))]
[RequireComponent(typeof(PlayerInputController))]
public class PlayerMachine : SuperStateMachine {

    public enum PlayerStates { Idle, Walk, Air, Sticky, Hoppy, NoControl, AirNoControl }
    
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
    public float MaxSuperJump = 10;
    public float SuperJumpBuildingSpeed = 1.0f;
    public float StickyTargetTime = 1.0f;

    //Private variables for different behaviours
    private float jumptime = 0;
    private bool CanDoubleJump = true;
    private RaycastHit StickWall;
    private Vector3 Lastmovedirection;
    private Vector3 moveDirection;
    private float SuperJumpCount = 0;
    private  Animator anim;  //Dee: animator
    private bool IsCharging = false;
    private bool IsSticking = false;
    private float StickyTimer = 0;


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

        transform.position += moveDirection * Time.deltaTime;
        
        if(InControl && input.Current.Recall)
        {
            leftSwitchTarget.transform.position = transform.position - AnimatedMesh.forward - AnimatedMesh.right;
            rightSwitchTarget.transform.position = transform.position - AnimatedMesh.forward + AnimatedMesh.right;
        }

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
            //transform.position += moveDirection * 0.02f;
            int derp = 42;
        }
        anim.SetFloat("Y Direction", moveDirection.y);
    }

    //Idle State
    void Idle_EnterState()
    {
        //Dee: ANIMATE
        anim.SetBool("IsWalking", false);
        anim.SetBool("Clamping", true);

        controller.EnableSlopeLimit();
        controller.EnableClamping();

        CanDoubleJump = true;
    }
    void Idle_SuperUpdate()
    {
        //Simoncode
        //Check if we're gonna jump
        if (input.Current.JumpInput)
        {
            currentState = PlayerStates.Air;
            Jump(JumpHeight, Gravity);
            return;
        }
        
        //Simoncode
        //Check if we're gonna fall
        if (!MaintainingGround())
        {
            currentState = PlayerStates.Air;
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
        //AnimatedMesh.rotation = Quaternion.FromToRotation(controller.up, controller.currentGround.PrimaryNormal());
        //AnimatedMesh.rotation = AnimatedMesh.rotation * Quaternion.LookRotation(Lastmovedirection, controller.up);
        if(moveDirection.magnitude > 0)
            AnimatedMesh.rotation = Quaternion.RotateTowards(AnimatedMesh.rotation, Quaternion.FromToRotation(controller.up, controller.currentGround.PrimaryNormal()) * Quaternion.LookRotation(Lastmovedirection, controller.up), 8);

        HandleHoppy();
        HandleSwitching();
    }

    //Walk State
    void Walk_EnterState()
    {
        anim.SetBool("Clamping", true);
        CanDoubleJump = true;
    }
    void Walk_SuperUpdate()
    {
        //Check if we're gonna jump
        if (input.Current.JumpInput)
        {
            currentState = PlayerStates.Air;
            Jump(JumpHeight, Gravity);
            return;
        }

        //Check if we're gonna fall
        if (!MaintainingGround())
        {
            currentState = PlayerStates.Air;
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
            //AnimatedMesh.rotation = Quaternion.FromToRotation(controller.up, controller.currentGround.PrimaryNormal());
            //AnimatedMesh.rotation = AnimatedMesh.rotation * Quaternion.LookRotation(moveDirection, controller.up);

            AnimatedMesh.rotation = Quaternion.RotateTowards(AnimatedMesh.rotation, Quaternion.FromToRotation(controller.up, controller.currentGround.PrimaryNormal()) * Quaternion.LookRotation(moveDirection, controller.up), 8);
            
            //AnimatedMesh.rotation = Quaternion.LookRotation(moveDirection, controller.up);
            //AnimatedMesh.rotation = Quaternion.RotateTowards(AnimatedMesh.rotation, AnimatedMesh.rotation * Quaternion.LookRotation(moveDirection, controller.up), 4);

        }
        else
        {
            //If we're standing still, we go over to the idle state
            currentState = PlayerStates.Idle;
            return;
        }

        HandleSwitching();
        HandleHoppy();
    }

    void Air_EnterState()
    {
        controller.DisableClamping();
        controller.DisableSlopeLimit();
        
        jumptime = 0;

        anim.SetBool("Clamping", false);

    }
    void Air_SuperUpdate()
    {
        jumptime += Time.deltaTime;
        if (jumptime < 0.1 && input.Current.JumpInput)
        {
            Jump(JumpHeight, Gravity);
            jumptime = 1;
            return;
        }

        HandleDoubleJump();
        if (HandleSticky())
            return;

        HandleAirMovement();
        HandleSwitching();
    }
    void Air_ExitState()
    {
        //Dee: ANIMATE!
        anim.SetBool("IsJumping", false);
        anim.SetBool("IsDoubleJumping", false);
        anim.SetBool("HasLanded", true);
        if(EnableGlidey)
           anim.SetBool("FoldIn", true);
        if(EnableSticky)
            anim.SetBool("IsJumpingFromStick", false);
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
        //AnimatedMesh.position -= Vector3.Scale(StickWall.normal * StickWall.distance, new Vector3(1, 0, 1));
        AnimatedMesh.position = StickWall.point;

        anim.SetBool("IsSticking", true);

        CanDoubleJump = true;
        IsSticking = true;
    }
    void Sticky_SuperUpdate()
    {
        //All we do in this state is checking if we should leave it
        if(input.Current.JumpInput)
        {
            currentState = PlayerStates.Air;
            Jump(JumpHeight, Gravity);
            moveDirection += StickWall.normal * 20;
            IsSticking = false;

        }
        else if(input.Current.Sticky)
        {
            currentState = PlayerStates.Air;
            moveDirection += StickWall.normal * 5;
            IsSticking = false;
        }

        HandleSwitching();
        //if(input.Current.MoveInput != Vector3.zero)
        //{
        //    moveDirection = Vector3.MoveTowards(moveDirection, LocalMovement() * WalkSpeed * input.moveinput.magnitude, WalkAcceleration * Time.deltaTime);
        //    moveDirection.y = 0;
        //}

        //RaycastHit hit;
        //Ray wallmove = new Ray(transform.position, -StickWall.normal);
        //Debug.DrawLine(transform.position, -StickWall.normal);
        //if(StickWall.collider.Raycast(wallmove, out hit, Mathf.Infinity))
        //{
        //    StickWall = hit;
        //    //We angle ourselves towards the wall and look upwards
        //    AnimatedMesh.rotation = Quaternion.FromToRotation(controller.up, StickWall.normal);
        //    AnimatedMesh.rotation = AnimatedMesh.rotation * Quaternion.LookRotation(Lastmovedirection, controller.up);

        //    //We push the slime towards the wall so it actually looks like we're sticking
        //    transform.position -= Vector3.Scale(StickWall.normal * StickWall.distance , new Vector3(1,0,1));
        //    AnimatedMesh.position = transform.position - StickWall.normal * 0.5f;
        //    Debug.Log(StickWall.distance);
        //}
        //else
        //{
        //    Debug.Log("OH NO DEBUG JOHN CENA!");
        //    //currentState = PlayerStates.Air;
        //}
    }
    void Sticky_ExitState()
    {
        //Dee: ANIMATE!
        anim.SetBool("IsSticking", false);
        anim.SetBool("IsJumpingFromStick", true);


        if (currentState.ToString() != "NoControl")
        {
            //When we leave the state, we leave with momemtum away from the wall
            AnimatedMesh.position = transform.position;

            //And we normalize our rotation
            AnimatedMesh.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    void Hoppy_EnterState()
    {
        moveDirection = Vector3.zero;
        AnimatedMesh.localScale = new Vector3(1, 0.5f, 1);

        anim.SetBool("IsSuperJumping", true);
    }
    void Hoppy_SuperUpdate()
    {
        if (MaxSuperJump > SuperJumpCount)
            SuperJumpCount += Time.deltaTime * SuperJumpBuildingSpeed;

        if(!input.Current.Debug && SuperJumpCount != 0)
        {
            currentState = PlayerStates.Air;
        }
    }
    void Hoppy_ExitState()
    {
        AnimatedMesh.localScale = new Vector3(1, 1, 1);
        Jump(SuperJumpCount, Gravity);
        SuperJumpCount = 0;

        anim.SetBool("IsSuperJumping", false);
    }

    //No Control State
    void NoControl_EnterState()
    {
        moveDirection = Vector3.zero;
    }
    void NoControl_SuperUpdate()
    {
        if(InControl)
        {
            if (IsSticking)
                currentState = PlayerStates.Sticky;
            else
                currentState = PlayerStates.Idle;

            return;
        }
        if(!MaintainingGround())
        {
            currentState = PlayerStates.AirNoControl;
        }
        if(!IsSticking)
        {
            Vector3 rotatetowardscharacter = controlTarget.transform.position - controller.transform.position;
            rotatetowardscharacter.y = 0;
            AnimatedMesh.rotation = Quaternion.RotateTowards(AnimatedMesh.rotation, Quaternion.LookRotation(rotatetowardscharacter), 3);
        }
    }
    void NoControl_ExitState()
    {

    }

    //Air No Control State
    void AirNoControl_EnterState()
    {
        controller.DisableClamping();
        controller.DisableSlopeLimit();

        anim.SetBool("Clamping", true);
    }
    void AirNoControl_SuperUpdate()
    {
        if(!IsSticking)
        {
            HandleAirMovement(false);
        }
        if(InControl)
        {
            if (IsSticking)
            {
                currentState = PlayerStates.Sticky;
            }
            else
            {
                currentState = PlayerStates.Air;
            }
            return;
        }
    }
    void AirNoControl_ExitState()
    {
        //Dee: ANIMATE!
        anim.SetBool("IsJumping", false);
        anim.SetBool("IsDoubleJumping", false);
        anim.SetBool("HasLanded", true);
        if (EnableGlidey)
            anim.SetBool("FoldIn", true);
        if (EnableGlidey)
            anim.SetBool("IsJumpingFromStick", false);
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
        //Dee: ANIMATE!
        anim.SetBool("IsJumping", true);
        anim.SetBool("HasLanded", false);
        if(EnableGlidey)
            anim.SetBool("FoldIn", false);

        moveDirection += controller.up * CalculateJumpSpeed(height, gravity);
    }


    //Private function used in this script which are executed continously
    private void HandleDoubleJump()
    {
        //Check jump input
        if (input.Current.JumpInput && CanDoubleJump)
        {
            //Dee: ANIMATE! this is actually making him play the animation TWICE. Why??
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
            //Dee: ANIMATE!
            anim.SetBool("IsGliding", true);

            return -Vector3.up * Glide;
        }
        if (EnableGlidey && anim.GetBool("IsGliding"))
        {
            anim.SetBool("FoldIn", true);
            anim.SetBool("IsGliding", false);
        }


        //DEE: ANIMATE!
        //Simon: I do fix
        return verticalmovement;
    }
    private bool HandleSticky()
    {

        //First we check the input and if we can stick
        if (input.Current.Sticky && EnableSticky)
        {
            StickyTimer = StickyTargetTime;
            ////Then we check if there are any colliders in a sphere around you
            //Collider[] colliders = Physics.OverlapSphere(controller.transform.position, 1);
            //foreach (Collider col in colliders)
            //{
            //    if (col.tag == "Environment")
            //    {
            //        //When we find a sticky wall we raycast towards it's center to get the normal
            //        Ray wallray = new Ray(controller.transform.position, col.bounds.center - controller.transform.position);
            //        RaycastHit hit;

            //        if (col.Raycast(wallray, out hit, Mathf.Infinity))
            //        {
            //            //Make a new ray with the direction of the wall's normal
            //            wallray = new Ray(controller.transform.position, -hit.normal);

            //            //Then we raycast towards the normal. This will be the closest point on the collider in almost every case.
            //            if (col.Raycast(wallray, out hit, Mathf.Infinity) && hit.normal.y < 1)
            //            {
            //                if (Vector3.Angle(controller.up, hit.normal) < 40.0f)
            //                    return false;

            //                StickWall = hit;
            //                currentState = PlayerStates.Sticky;
            //                return true;
            //            }
            //        }
            //    }
            //}
        }
        if (StickyTimer > 0)
        {
            StickyTimer -= Time.deltaTime;

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
                            if (Vector3.Angle(controller.up, hit.normal) < 40.0f)
                                return false;

                            StickWall = hit;
                            currentState = PlayerStates.Sticky;
                            StickyTimer = 0;
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    private void HandleHoppy()
    {
        if(input.Current.Debug && EnableHoppy)
        {
            currentState = PlayerStates.Hoppy;
        }
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
            if (currentState.ToString() != "Air" || IsSticking)
                currentState = PlayerStates.NoControl;
            else
                currentState = PlayerStates.AirNoControl;
        }
        else if (input.Current.RightBumper)
        {
            //Enable playercontroller on specified
            rightSwitchTarget.GetComponent<PlayerMachine>().InControl = true;

            //set new camera target
            camera.GetComponent<PlayerCamera>().SetTarget(rightSwitchTarget);

            EventSystem.ActivateSwitchCharacter(rightSwitchTarget);

            InControl = false;
            if (currentState.ToString() != "Air" || IsSticking)
                currentState = PlayerStates.NoControl;
            else
                currentState = PlayerStates.AirNoControl;
        }

    }
    private void HandleAirMovement(bool enablemovement = true)
    {
        //This is unneccesary and should be rewritten :p :p :p
        Vector3 planarMoveDirection = Math3d.ProjectVectorOnPlane(controller.up, moveDirection);
        Vector3 verticalMoveDirection = moveDirection - planarMoveDirection;

        //This calculates if we touch the ground
        if (Vector3.Angle(verticalMoveDirection, controller.up) > 90 && AcquiringGround())
        {
            moveDirection = planarMoveDirection;
            if (InControl)
                currentState = PlayerStates.Idle;
            else
                currentState = PlayerStates.NoControl;

            moveDirection *= Slowdown;
            return;
        }

        if(enablemovement && input.moveinput.magnitude > 0.2)
        {
            planarMoveDirection = Vector3.MoveTowards(planarMoveDirection, LocalMovement() * WalkSpeed, JumpAcceleration * Time.deltaTime);
        }
        if (verticalMoveDirection.y > -30)
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
        if (tempdirection != Vector3.zero)
        {
            //AnimatedMesh.rotation = Quaternion.RotateTowards(AnimatedMesh.rotation, Quaternion.LookRotation(tempdirection, controller.up) * Quaternion.RotateTowards(controller.up, controller.up), 8);
            AnimatedMesh.rotation = Quaternion.RotateTowards(AnimatedMesh.rotation, Quaternion.FromToRotation(controller.up, controller.up) * Quaternion.LookRotation(tempdirection, controller.up), 8);
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
        anim.SetBool("IsDoubleJumping", false);
    }
    public void ForceSwitch(Transform target)
    {
        if (target == transform)
            return;

        //Enable playercontroller on specified
        target.GetComponent<PlayerMachine>().InControl = true;

        //set new camera target
        camera.GetComponent<PlayerCamera>().SetTarget(target);

        EventSystem.ActivateSwitchCharacter(target);

        InControl = false;
        currentState = PlayerStates.NoControl;
    }
    public void ChangeState(PlayerStates states)
    {
        currentState = states;
    }
}
