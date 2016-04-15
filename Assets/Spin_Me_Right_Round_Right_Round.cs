using UnityEngine;
using System.Collections;

public class Spin_Me_Right_Round_Right_Round : MonoBehaviour {

    public float maxLeftRotate = 40.0f;
    public float maxRightRotate = 40.0f;
    public float moveSpeed = 0.5f;

    public bool limited = true;
    public bool reverse = false;

    private float spin;

    // Use this for initialization
    void Start ()
    {
        if (reverse == true) moveSpeed = -moveSpeed;
	}
	
	// Update is called once per frame
	void Update ()
    {

        spin += moveSpeed;

        if (limited == true) spin = Mathf.Clamp(spin, maxLeftRotate, maxRightRotate);


        //Vector3 around = Vector3.;
        transform.rotation = Quaternion.AngleAxis(spin, Vector3.up) * transform.rotation;
    }
}
