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
    private float limits;

    private Vector3 aroundSpin;

    // Use this for initialization
    void Start ()
    {
        if (reverse == true) moveSpeed = -moveSpeed;

        spin = moveSpeed;

    }

    // Update is called once per frame
    void Update ()
    {

        limits = 180/Mathf.PI * thisOne.transform.rotation.y*2;

        if (limited == true)
        {
            limits = Mathf.Clamp(limits, -maxLeftRotate, maxRightRotate);

            if (limits == maxRightRotate) spin = -spin;
            if (limits == -maxLeftRotate) spin = -spin;
        }

        transform.rotation = Quaternion.AngleAxis(spin, Vector3.up) * transform.rotation;
    }
}
