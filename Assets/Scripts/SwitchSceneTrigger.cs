using UnityEngine;
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

            UnityEngine.SceneManagement.SceneManager.LoadScene(LevelName);
        }
    }
}
