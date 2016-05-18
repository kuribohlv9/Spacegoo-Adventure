using UnityEngine;
using System.Collections;

public class PortalScriptForYouPony : MonoBehaviour {

    public GameObject TeleTarget;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            col.transform.parent.transform.position = TeleTarget.transform.position;
        }
    }
}
