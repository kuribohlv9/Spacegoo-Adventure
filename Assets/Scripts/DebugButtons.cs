using UnityEngine;
using System.Collections;

public class DebugButtons : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetButtonDown("Debug F1"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        if (Input.GetButtonDown("Debug F2"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
	}
}
