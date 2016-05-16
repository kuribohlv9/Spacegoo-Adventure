using UnityEngine;
using System.Collections;

public class PlayerInputController : MonoBehaviour {

    public PlayerInput Current;
    public Vector3 moveinput;
    public string PlayerNumber = "1";

	// Use this for initialization
	void Start () {
        Current = new PlayerInput();
	}
	
	// Update is called once per frame
	void Update () {
        

        // Retrieve our current WASD or Arrow Key input
        // Using GetAxisRaw removes any kind of gravity or filtering being applied to the input
        // Ensuring that we are getting either -1, 0 or 1   
        //Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveInput = new Vector3(Input.GetAxis("Left Stick X" + PlayerNumber), 0, Input.GetAxis("Left Stick Y" + PlayerNumber));
        moveinput = moveInput;
        Vector2 mouseInput = new Vector2(Input.GetAxis("Right Stick X" + PlayerNumber), Input.GetAxis("Right Stick Y" + PlayerNumber));

        if(mouseInput.magnitude < 0.01f)
        {
            mouseInput = Vector2.zero;
        }

        bool jumpInput = Input.GetButtonDown("A Button" + PlayerNumber);
        bool continuousJumpInput = Input.GetButton("A Button" + PlayerNumber);
        
        if(jumpInput == true)
        {
            int derp = 42;
        }
        bool sticky = Input.GetButtonDown("X Button" + PlayerNumber);
        bool debug = Input.GetButton("B Button" + PlayerNumber);
        bool recall = Input.GetButtonDown("Y Button" + PlayerNumber);
        bool leftbumper = Input.GetButtonDown("Swap Left" + PlayerNumber);
        bool rightbumper = Input.GetButtonDown("Swap Right" + PlayerNumber);

        Current = new PlayerInput()
        {
            MoveInput = moveInput,
            MouseInput = mouseInput,
            JumpInput = jumpInput,
            ContinuousJumpInput = continuousJumpInput,
            Sticky = sticky,
            Debug = debug,
            LeftBumper = leftbumper,
            RightBumper = rightbumper,
            Recall = recall
            
        };
	}
}

public struct PlayerInput
{
    public Vector3 MoveInput;
    public Vector2 MouseInput;
    public bool JumpInput;
    public bool ContinuousJumpInput;
    public bool Sticky;
    public bool Debug;
    public bool LeftBumper;
    public bool RightBumper;
    public bool Recall;
}
