using UnityEngine;
using System.Collections;

public class GitDNet : MonoBehaviour
{

    public int Number;
    public Color glowColor = new Vector4(0.569F, 1, 1, 1);

    private Light glowLight;

    private float timer = 0;
    public float timerTarget = 0.0f;


    // Use this for initialization
    void Start()
    {

        glowLight = GetComponent<Light>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LightUp()
    {

        Renderer rend = GetComponentInChildren<Renderer>();

        rend.material.shader = Shader.Find("Standard");         //This is magically always using the right shader, regardless of order of material. Might be based on mesh where the script is attached (I put it on hats)  http://answers.unity3d.com/questions/960607/how-to-material-in-emission-color-change-1.html#comment-994024 
        rend.material.SetColor("_EmissionColor", glowColor);

        if (!glowLight.isActiveAndEnabled)
        {
            glowLight.enabled = !glowLight.enabled;               //THIS IS THE LIGHT FROM THE SHROOM. NECESSARY. DO NOT KILL
          
        }
        
    }

    public void LightOff()
    {

        Renderer rend = GetComponentInChildren<Renderer>();

        rend.material.shader = Shader.Find("Standard");
        rend.material.SetColor("_EmissionColor", Color.black);      //Making the emission black is FASTER than turning it off (according to forum guy)

        glowLight.enabled = !glowLight.enabled;               //THIS IS THE LIGHT FROM THE SHROOM. NECESSARY. DO NOT KILL

    }

    void OnTriggerEnter(Collider col)                   //CHANGE THIS TO ONSTAY, GLOWS WHEN PLAYER MOVES ON IT for 1.5s
    {
        if (col.tag == "Player")
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
        if (col.tag == "Player")
        {
            LightOff();
        }
    }
}






//using UnityEngine;
//using System.Collections;

//public class GitDNet : MonoBehaviour
//{

//    public int Number;
//    public Color glowColor = new Vector4(0.569F, 1, 1, 1);

//    private Light glowLight;

//    private float timer = 0;
//    public float timerTarget = 0.0f;


//    // Use this for initialization
//    void Start()
//    {

//        glowLight = GetComponent<Light>();

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    public void LightUp()
//    {

//        Renderer rend = GetComponentInChildren<Renderer>();

//        rend.material.shader = Shader.Find("Standard");         //This is magically always using the right shader, regardless of order of material. Might be based on mesh where the script is attached (I put it on hats)  http://answers.unity3d.com/questions/960607/how-to-material-in-emission-color-change-1.html#comment-994024 
//        rend.material.SetColor("_EmissionColor", glowColor);

//         if (!glowLight.isActiveAndEnabled)
        //{
        //    glowLight.enabled = !glowLight.enabled;               //THIS IS THE LIGHT FROM THE SHROOM. NECESSARY. DO NOT KILL
          
        //}
//    }

//    public void LightOff()
//    {

//        Renderer rend = GetComponentInChildren<Renderer>();

//        rend.material.shader = Shader.Find("Standard");
//        rend.material.SetColor("_EmissionColor", Color.black);      //Making the emission black is FASTER than turning it off (according to forum guy)

//        glowLight.enabled = !glowLight.enabled;               //THIS IS THE LIGHT FROM THE SHROOM. NECESSARY. DO NOT KILL

//    }

//    void OnAwake()
//    {
//        //This subscribes the function LightUp to the onglownet event
//        EventSystem.onglownet += LightUp;       //NOTE! NOT AS A FUNCTION! it's actually a variable!

//    }

//    void OnTriggerEnter(Collider col)                   //CHANGE THIS TO ONSTAY, GLOWS WHEN PLAYER MOVES ON IT for 1.5s
//    {
//        if (col.tag == "Player")
//        {
//            timer += Time.deltaTime;

//            if (timer > timerTarget)
//            {
//                EventSystem.ActivateGlowNet();
//                //LightUp();
//            }

//        }
//    }

//    void OnTriggerExit(Collider col)
//    {
//        if (col.tag == "Player")
//        {
//            LightOff();
//        }
//    }
//}
////EXAMPLE TIME

////If we have the center mushroom's on player neter function here
////private void OnTriggerEnter(Collider col)
////{
////  if(col.tag == "Player")
////  {
////      //Call this static function when the player enters the múshroom.
////      //This will activate the function in Eventsystem above^
////      ActivateOnDoubleJump();
////  }
////}

////Over here in each mushroom script we have the lughtup function
////public void LightUp()
////{
////}
////It does things

////But above it we need to assign it to ondoublejump
////public void OnAwake()
////{
////  //This subscribes the function LightUp to the ondoublejump event
////  EventSystem.ondoublejump += Lightup();
////}

////So when the center mushroom calls ActivateOnDoubleJump, each mushroom will exeute the LightUp function.
////You can create new event variables and new activation functions. Just copy everything above and change the name
////OnDoubleJump and ondoublejump to the things you wanna call them.
////Good luck!

//// IF U GET STUCK, LOOK AT SIMON'S TURN TO FACE CURRENTLY ACTIVE CHARACTER EVENT!
