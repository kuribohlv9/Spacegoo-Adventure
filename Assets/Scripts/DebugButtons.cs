using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DebugButtons : MonoBehaviour {

    public GameObject PauseScreen;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetButtonDown("Debug F1"))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetButtonDown("Debug F2"))
        {
            SceneManager.LoadScene(1);
        }
        if(Input.GetButtonDown("Start1"))
        {
            if(Time.timeScale == 1)
            {
                PauseScreen.SetActive(true);
            }
            else if(Time.timeScale == 0)
            {
                PauseScreen.SetActive(false);
            }
        }
	}
}
