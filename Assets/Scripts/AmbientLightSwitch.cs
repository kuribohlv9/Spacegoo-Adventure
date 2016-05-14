using UnityEngine;
using System.Collections;

public class AmbientLightSwitch : MonoBehaviour {

    private bool InControlIsHere = false;
    private bool IsDark = false;
    public GameObject lightGameObject;

    private float AreaambientIntensity;
    private float AreareflectionIntensity;
    private Color AreaambientEquatorColor;
    private Color AreaambientGroundColor;
    private Color AreaambientSkyColor;
	// Use this for initialization
	void Start () {

        AreaambientIntensity = RenderSettings.ambientIntensity;
        AreareflectionIntensity = RenderSettings.reflectionIntensity;

        AreaambientEquatorColor = RenderSettings.ambientEquatorColor;
        AreaambientGroundColor = RenderSettings.ambientGroundColor;
        AreaambientSkyColor = RenderSettings.ambientSkyColor;
	}
	
	// Update is called once per frame
	void Update () {

	}

    void LateUpdate()
    {
        if(InControlIsHere == false && IsDark)
        {
            TurnBright();
        }
        else if(InControlIsHere && IsDark == false)
        {
            TurnDark();
        }
        InControlIsHere = false;
    }

    void OnTriggerEnter(Collider col)
    {
        //Debug.Log("John");
        if(col.tag == "Player")
        {
            TurnDark();
        }
    }
    void OnTriggerStay(Collider col)
    {
        if(col.tag == "Player")
        {
            if (col.GetComponentInParent<PlayerMachine>().InControl == false)
                return;

            InControlIsHere = true;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            TurnBright();
        }
    }
    private void TurnDark()
    {
        RenderSettings.ambientIntensity = 0.33f;
        RenderSettings.reflectionIntensity = 0.038f;

        RenderSettings.ambientEquatorColor = Color.black;
        RenderSettings.ambientGroundColor = Color.black;
        RenderSettings.ambientSkyColor = Color.black;

        IsDark = true;
        lightGameObject.SetActive(false);
    }
    private void TurnBright()
    {
        RenderSettings.ambientIntensity = AreaambientIntensity;
        RenderSettings.reflectionIntensity = AreareflectionIntensity;

        RenderSettings.ambientEquatorColor = AreaambientEquatorColor;
        RenderSettings.ambientGroundColor = AreaambientGroundColor;
        RenderSettings.ambientSkyColor = AreaambientSkyColor;

        IsDark = false;
        lightGameObject.SetActive(true);
    }
}
