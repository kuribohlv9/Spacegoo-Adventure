using UnityEngine;
using System.Collections;

public class BounceMultiTarget : MonoBehaviour {

    public BounceMultiRelay Relay;

    public float Height = 10;
    public Vector3 direction = new Vector3(0, 1, 0);
    public bool remoteTriggered = false;

    // Use this for initialization
    void Start ()
    {
        direction.Normalize();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (remoteTriggered != Relay.triggerHere) remoteTriggered = Relay.triggerHere;
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player" && remoteTriggered == true)
        {
            col.GetComponentInParent<PlayerMachine>().ChangeMovement(direction * Height);
        }
    }
}
