using UnityEngine;
using System.Collections;

public class GitDNet : MonoBehaviour {


    public Color glowColor = new Vector4(0.569F, 1, 1, 1);
    public Color capColor = new Vector4(0, 255, 9, 225);


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collisionInfo)       //CHANGE TO ONSTAY, CHANGE TO A TIMER.
    {
        if (collisionInfo.gameObject.tag == "Player")
        {
            Renderer rend = GetComponentInChildren<Renderer>();
            //foreach(Material matt in rend.materials)
            //{
            //    if(matt.name == "New Material 8")
            //    {
            //        //rend.material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            //        matt.shader = Shader.Find("Standard");
            //        matt.SetColor("_Color", Color.cyan);
            //    }
            //}
            //rend.material.SetColor("-EmissionColor", Color.cyan);

            rend.material.shader = Shader.Find("Standard");         //This will take the shader on the FIRST MATERIAL on the object the script is attached to!
            rend.material.SetColor("_EmissionColor", glowColor);

            //DynamicGI.UpdateMaterials(rend);
            //DynamicGI.UpdateEnvironment();
        }
    }

    void OnCollisionExit(Collision collisionInfo)       //CHANGE TO ONSTAY, CHANGE TO A TIMER.
    {
        if (collisionInfo.gameObject.tag == "Player")
        {
            Renderer rend = GetComponentInChildren<Renderer>();

            rend.material.shader = Shader.Find("Standard");
            rend.material.SetColor("_EmissionColor", capColor);

        }
    }
}
