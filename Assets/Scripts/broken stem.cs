using UnityEngine;
using System.Collections;

public class brokenstem : MonoBehaviour {

    AudioSource audio;
    public float Height = 10;

    public Rigidbody target1;
    public Rigidbody target2;

    bool colSolid = false;

    // Use this for initialization
    void Start ()
    {
        audio = GetComponent<AudioSource>();
        target1.useGravity = false;
        //target2.useGravity = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (colSolid == true) target1.useGravity = true;
        //if (colSolid == true) target2.useGravity = true;
    }

    /*void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            //audio.Play();
            //col.GetComponentInParent<PlayerMachine>().ChangeMovement(Vector3.up * Height);
            colSolid = true;
        }
    }*/

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            target1.useGravity = true;
            target1.isKinematic = false;
        }
    }
}
