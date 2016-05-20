using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SwitchSceneTrigger : MonoBehaviour {

    public Image loadingscreen;
    public int LevelNumber = 3;

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
            loadingscreen.gameObject.SetActive(true);
            SceneManager.LoadScene(LevelNumber);
        }
    }
}
