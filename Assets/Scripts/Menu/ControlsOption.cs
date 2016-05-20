using UnityEngine;
using System.Collections;

public class ControlsOption : MenuOption {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        GetComponentInParent<StartMenu>().SetSuperLock(true);
	}
}
