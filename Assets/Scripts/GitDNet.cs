using UnityEngine;
using System.Collections;

public class GitDNet : MonoBehaviour {


    public Color glowColor = new Vector4(0.569F, 1, 1, 1);


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)                   //CHANGE THIS TO ONSTAY, GLOWS WHEN PLAYER MOVES ON IT for 1.5s
    {
        if(col.tag == "Player")
        {
            Renderer rend = GetComponentInChildren<Renderer>();

            rend.material.shader = Shader.Find("Standard");         //This will take the shader on the FIRST MATERIAL on the object the script is attached to! http://answers.unity3d.com/questions/960607/how-to-material-in-emission-color-change-1.html#comment-994024 
            rend.material.SetColor("_EmissionColor", glowColor);

        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.tag == "Player")
        {
            Renderer rend = GetComponentInChildren<Renderer>();

            rend.material.shader = Shader.Find("Standard");
            rend.material.SetColor("_EmissionColor", Color.black);      //Making the emission black is FASTER than turning it off (according to forum guy)

        }
    }
}
