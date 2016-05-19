using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SwitchSceneTrigger : MonoBehaviour {

    public string LevelName = "";

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
            if(LevelName == "")
                return;

            SceneManager.LoadScene(LevelName);
        }
    }
}
