using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndScreenScene : MonoBehaviour {

    public string button;
    public Text instruction;
    public bool startfade = false;
    public float looptime = 1f;

    private bool complete = false;
    private float timer = 3.2f;
	
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
                timer = looptime;
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
