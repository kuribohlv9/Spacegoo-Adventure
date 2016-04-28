using UnityEngine;
using System.Collections;

public class EnableSwitchingTrigger : MonoBehaviour {

    public Transform target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            col.GetComponentInParent<PlayerMachine>().ForceSwitch(target);
        }
    }
}
