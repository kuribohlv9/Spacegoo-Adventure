using UnityEngine;
using System.Collections;

public class AmbientLightSwitch : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("John");
        if(col.tag == "Player")
        {
            Debug.Log("Cena");
            RenderSettings.ambientIntensity = 0.05f;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            RenderSettings.ambientIntensity = 1;
        }
    }
}
