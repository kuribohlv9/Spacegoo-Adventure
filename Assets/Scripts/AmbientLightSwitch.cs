﻿using UnityEngine;
using System.Collections;

public class AmbientLightSwitch : MonoBehaviour {

    private bool IsDark = false;

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
            RenderSettings.ambientIntensity = 0.0f;
        }
    }
    void OnTriggerStay(Collider col)
    {
        if(col.tag == "Player")
        {
            if(col.GetComponentInParent<PlayerMachine>().InControl && IsDark == false)
            {
                IsDark = true;
                RenderSettings.ambientIntensity = 0.0f;
            }
            else if (col.GetComponentInParent<PlayerMachine>().InControl == false && IsDark)
            {
                IsDark = false;
                RenderSettings.ambientIntensity = 1;
            }
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
