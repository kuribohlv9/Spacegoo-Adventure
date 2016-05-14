//using UnityEngine;
//using System.Collections;
using UnityEngine;
using System.Collections;

public class GitDNet : MonoBehaviour
{

    public int Number;
    public Color glowColor = new Vector4(0.569F, 1, 1, 1);

    public Color lerpedColor = new Vector4(0.569F, 1, 1, 1);


    public int IsHeadHoncho = 0;

    private Light glowLight;

    private float timer = 0;
    public float timerTarget = 0.0f;
    public float timerOffTarget = 0.0f;


    private bool IsLit = false;
    private bool willlightup = false;

    void OnEnable()
    {
        EventSystem.onglownet += WillLight;
    }
    void OnDisable()
    {
        EventSystem.onglownet -= WillLight;
    }
    // Use this for initialization
    void Start()
    {

        glowLight = GetComponent<Light>();

    }

    // Update is called once per frame
    void Update()
    {
        {


            //if (!IsLit && timer < timerTarget)
            //{
            //    timer += Time.deltaTime;
            //}

            //else if (!IsLit && timer > timerTarget)
            //{

            //                                    //here it should light up. one at a time
            //}


            //if willightup is true then timer than lightup()

            if ((willlightup || IsLit))
            {
                timer += Time.deltaTime;
            }

            if (willlightup && timer > timerTarget)
            {
                LightUp();
                willlightup = false;
                IsLit = true;                
                timer = 0;
            }
            else if (IsLit && timer > timerOffTarget)
            {
                Debug.Log("Sees that timerOffTarget has been reached");

                LightOff();
                IsLit = false;
                timer = 0;
            }
        }
    }

    private void WillLight()
    {

        willlightup = true;
        timer = 0;
    }

    public void LightUp()
    {

        Debug.Log("Goes into LightOn()");

        Renderer rend = GetComponentInChildren<Renderer>();

        //lerpedColor = Color.Lerp(Color.blue, glowColor, Mathf.PingPong(Time.time, 1));

        rend.material.shader = Shader.Find("Standard");         //This is magically always using the right shader, regardless of order of material. Might be based on mesh where the script is attached (I put it on hats)  http://answers.unity3d.com/questions/960607/how-to-material-in-emission-color-change-1.html#comment-994024 
        rend.material.SetColor("_EmissionColor", glowColor);

        if (!glowLight.isActiveAndEnabled)
        {
            glowLight.enabled = !glowLight.enabled;               //THIS IS THE LIGHT FROM THE SHROOM. NECESSARY. DO NOT KILL

        }
    }

    public void LightOff()
    {
        Debug.Log("Goes into LightOff()");

        Renderer rend = GetComponentInChildren<Renderer>();

        rend.material.shader = Shader.Find("Standard");
        rend.material.SetColor("_EmissionColor", Color.black);      //Making the emission black is FASTER than turning it off (according to forum guy)

        glowLight.enabled = !glowLight.enabled;               //THIS IS THE LIGHT FROM THE SHROOM. NECESSARY. DO NOT KILL

    }


  
    void OnTriggerEnter(Collider col)                   //CHANGE THIS TO ONSTAY, GLOWS WHEN PLAYER MOVES ON IT for 1.5s
    {
        if (col.tag == "Player" && IsHeadHoncho >= 1)
        {
        //Debug.Log("JOHN");
            
           
           timer += Time.deltaTime;
                    

            EventSystem.ActivateGlowNet();

            
        }

    }
}


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
