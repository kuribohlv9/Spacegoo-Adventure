using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class XDoesWhat : MonoBehaviour {

    public Transform Sticky;
    public Text output;

    private int which = 0;

    // Use this for initialization
    void Start ()
    {
        if (InsertPlayerInSlime(Sticky) && which != 1)
        {
            output.text = "Stick";
            which = 1;
        }

        if (InsertPlayerInSlime(Sticky.GetComponent<PlayerMachine>().rightSwitchTarget) && which != 2)
        {
            output.text = "Glide";
            which = 2;
        }

        if (InsertPlayerInSlime(Sticky.GetComponent<PlayerMachine>().rightSwitchTarget.GetComponent<PlayerMachine>().rightSwitchTarget) && which != 3)
        {
            output.text = "Power Jump";
            which = 3;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private bool InsertPlayerInSlime(Transform target)
    {
        if (target.GetComponent<PlayerMachine>().InControl)
        {
            return true;
        }
        return false;
    }
}
