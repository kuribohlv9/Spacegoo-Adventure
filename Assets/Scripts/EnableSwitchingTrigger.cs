using UnityEngine;
using System.Collections;

public class EnableSwitchingTrigger : MonoBehaviour {

    public Transform target;

    private bool hasTriggered = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player" && !hasTriggered)
        {
            hasTriggered = true;
            col.GetComponentInParent<PlayerMachine>().ForceSwitch(target);
        }
    }
}
