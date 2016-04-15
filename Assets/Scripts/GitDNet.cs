using UnityEngine;
using System.Collections;

public class GitDNet : MonoBehaviour {

    public int Number;
    public Color glowColor = new Vector4(0.569F, 1, 1, 1);

    private Light glowLight;

    private float timer = 0;
    public float timerTarget = 0.0f;


	// Use this for initialization
	void Start () {

        glowLight = GetComponent<Light>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LightUp()
    {

        Renderer rend = GetComponentInChildren<Renderer>();

        rend.material.shader = Shader.Find("Standard");         //This is magically always using the right shader, regardless of order of material. Might be based on mesh where the script is attached (I put it on hats)  http://answers.unity3d.com/questions/960607/how-to-material-in-emission-color-change-1.html#comment-994024 
        rend.material.SetColor("_EmissionColor", glowColor);

        glowLight.enabled = !glowLight.enabled;               //THIS IS THE LIGHT FROM THE SHROOM. NECESSARY. DO NOT KILL
        //make this on a timer
    }

    void LightOff()
    {

        Renderer rend = GetComponentInChildren<Renderer>();

        rend.material.shader = Shader.Find("Standard");
        rend.material.SetColor("_EmissionColor", Color.black);      //Making the emission black is FASTER than turning it off (according to forum guy)

        glowLight.enabled = !glowLight.enabled;               //THIS IS THE LIGHT FROM THE SHROOM. NECESSARY. DO NOT KILL

    }

    //make one function for handling light, call it when movement on shroom 1 is detected
    //do below, shader emission and light enable on each shroom, based on a timer assigned to each shroom's number like case: Number == 2, wait 1s, case: Number ==3, wait 2.5s and so on
    //if no movement then turn off, OR wait first and then turn off?? I think the latter...

    void OnTriggerEnter(Collider col)                   //CHANGE THIS TO ONSTAY, GLOWS WHEN PLAYER MOVES ON IT for 1.5s
    {
        if(col.tag == "Player")
        {
            timer += Time.deltaTime;

            if (timer > timerTarget)
            {
                LightUp();
            }

        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.tag == "Player")
        {
            LightOff();
        }
    }
}
