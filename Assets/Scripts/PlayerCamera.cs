using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

    public float Distance = 5.0f;
    public float Height = 2.0f;
    public float xRotationSpeed = 15.0f;
    public float yRotationSpeed = 4.0f;


    public float maxDownRotate = 10.0f;
    public float maxUpRotate = 75.0f;

    public GameObject PlayerTarget;    

    private PlayerInputController input;
    private Transform target;
    private PlayerMachine machine;
    private float yRotation;
    private float xRotation;

    private SuperCharacterController controller;

	// Use this for initialization
	void Start () {
        input = PlayerTarget.GetComponent<PlayerInputController>();
        machine = PlayerTarget.GetComponent<PlayerMachine>();
        controller = PlayerTarget.GetComponent<SuperCharacterController>();
        target = PlayerTarget.transform;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = target.position;

        yRotation += input.Current.MouseInput.y * yRotationSpeed;

        yRotation = Mathf.Clamp(yRotation, -maxUpRotate, maxDownRotate);

        xRotation += input.Current.MouseInput.x;

        Debug.Log(yRotation);

        Vector3 upward = Vector3.Cross(machine.lookDirection, controller.up);
        Vector3 right = Vector3.Cross(machine.lookDirection, controller.right);

        transform.rotation = Quaternion.LookRotation(machine.lookDirection, controller.up);
        //transform.rotation = Quaternion.LookRotation(machine.lookDirection, controller.right);
        transform.rotation = Quaternion.AngleAxis(yRotation, upward) * transform.rotation;
        transform.rotation = Quaternion.AngleAxis(xRotation * xRotationSpeed, right) * transform.rotation;

        

        transform.position -= transform.forward * Distance;
        transform.position += controller.up * Height;

	}
}
