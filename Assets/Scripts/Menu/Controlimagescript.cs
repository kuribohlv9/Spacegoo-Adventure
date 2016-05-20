using UnityEngine;
using System.Collections;

public class Controlimagescript : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("X Button1"))
        {
            GetComponentInParent<StartMenu>().SetSuperLock(false);
            gameObject.SetActive(false);
        }
	}
}
