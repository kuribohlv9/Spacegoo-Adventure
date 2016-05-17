using UnityEngine;
using System.Collections;

public class Spin_Me_Right_Round_Right_Round : MonoBehaviour {

    public GameObject thisOne;

    public float maxLeftRotate = 40.0f;
    public float maxRightRotate = 40.0f;
    public float moveSpeed = 0.5f;

    public bool limited = true;
    public bool reverse = false;

    private float spin;
    private float spinForce;
    private float limits;
    private float maxLeft;
    private float maxRight;
    private float startRotation;


    private Vector3 aroundSpin;

    // Use this for initialization
    void Start ()
    {
        if (reverse == true) moveSpeed = -moveSpeed;

        spin = moveSpeed;
        spinForce = spin * Time.deltaTime * 10;

        startRotation = transform.rotation.y;
    }

    // Update is called once per frame
    void Update ()
    {

        maxLeft = startRotation + maxLeftRotate;
        maxRight = startRotation + maxRightRotate;

        limits = 180/Mathf.PI * thisOne.transform.rotation.y*2;

        if (limited == true)
        {
            limits = Mathf.Clamp(limits, -maxLeftRotate, maxRightRotate);

            if (limits == maxRight) spin = -spin;
            if (limits == -maxLeft) spin = -spin;
        }

        spinForce = spin * Time.deltaTime * 10;

        transform.rotation = Quaternion.AngleAxis(spinForce, Vector3.up) * transform.rotation;
    }
}
