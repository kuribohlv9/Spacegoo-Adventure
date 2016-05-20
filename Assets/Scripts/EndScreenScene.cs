using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndScreenScene : MonoBehaviour {

    public string button;
    public Text instruction;
    public bool startfade = false;

    private bool complete = false;
    private float timer = 3;
	
	// Update is called once per frame
	void Update () {
        if(startfade)
        {
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                complete = true;
                instruction.enabled = true;
                startfade = false;
            }
        }
        if(complete)
        {
            if(Input.GetButtonDown(button))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
        }
	}
}
