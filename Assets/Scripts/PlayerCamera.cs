using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

    public float Distance = 5.0f;
    public float Height = 2.0f;
    public float xRotationSpeed = 30.0f;
    public float yRotationSpeed = 15.0f;


    public float maxDownRotate = 40.0f;
    public float maxUpRotate = 75.0f;

    public GameObject PlayerTarget;    

    private PlayerInputController input;
    public Transform target;
    private PlayerMachine machine;
    private float yRotation;
    private float xRotation;

    private float updateTime;

    private SuperCharacterController controller;

	// Use this for initialization
	void Start () {
        input = PlayerTarget.GetComponent<PlayerInputController>();
        machine = PlayerTarget.GetComponent<PlayerMachine>();
        controller = PlayerTarget.GetComponent<SuperCharacterController>();
        target = PlayerTarget.transform;
	}

    // Update is called once per frame
    void LateUpdate() {
        transform.position = target.position;

        yRotation += input.Current.MouseInput.y * yRotationSpeed * Time.deltaTime * 40;

        yRotation = Mathf.Clamp(yRotation, -maxUpRotate, maxDownRotate);

        xRotation += input.Current.MouseInput.x * xRotationSpeed * Time.deltaTime * 40;

        //Debug.Log(yRotation);

        /*if (Physics.Linecast(this.transform.position, PlayerTarget.transform.position))
        {
<<<<<<< HEAD
            //Debug.Log("blocked");
        }
=======
            Debug.Log("blocked");
        }*/
>>>>>>> refs/remotes/origin/Stefan7IReallyNeedToStopMakingBranches

        Vector3 upward = Vector3.Cross(machine.lookDirection, controller.up);
        Vector3 right = Vector3.Cross(machine.lookDirection, controller.right);

        transform.rotation = Quaternion.LookRotation(machine.lookDirection, controller.up);
        //transform.rotation = Quaternion.LookRotation(machine.lookDirection, controller.right);
        transform.rotation = Quaternion.AngleAxis(yRotation, upward) * transform.rotation;
        transform.rotation = Quaternion.AngleAxis(xRotation, right) * transform.rotation;

        transform.position -= transform.forward * Distance;
        transform.position += controller.up * Height;
        


	}
    public void SetTarget(Transform newtarget)
    {
        target = newtarget;
        input = target.GetComponent<PlayerInputController>();
        machine = target.GetComponent<PlayerMachine>();
        controller = target.GetComponent<SuperCharacterController>();
    }
}
