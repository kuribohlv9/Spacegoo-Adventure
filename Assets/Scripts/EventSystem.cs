using UnityEngine;
using System.Collections;

public class EventSystem
{

    public delegate void OnDoubleJump();
    public static event OnDoubleJump ondoublejump;

    public delegate void OnSwitchCharacter(Transform target);
    public static event OnSwitchCharacter onswitchcharacter;

    public delegate void OnGlowNet();
    public static event OnGlowNet onglownet;

    public static void ActivateOnDoubleJump()
    {
        if (ondoublejump != null)
            ondoublejump();
    }

    public static void ActivateSwitchCharacter(Transform target)
    {
        if (onswitchcharacter != null)
            onswitchcharacter(target);
    }

    public static void ActivateGlowNet()
    {
            Debug.Log("CENA");
        if (onglownet != null)
        {
            Debug.Log("DUDURUDU");
            onglownet();
        }
    }

    //EXAMPLE TIME

    //If we have the center mushroom's on player neter function here
    //private void OnTriggerEnter(Collider col)
    //{
    //  if(col.tag == "Player")
    //  {
    //      //Call this static function when the player enters the múshroom.
    //      //This will activate the function in Eventsystem above^
    //      ActivateOnDoubleJump();
    //  }
    //}

    //Over here in each mushroom script we have the lughtup function
    //public void LightUp()
    //{
    //}
    //It does things

    //But above it we need to assign it to ondoublejump
    //public void OnAwake()
    //{
    //  //This subscribes the function LightUp to the ondoublejump event
    //  EventSystem.ondoublejump += Lightup();
    //}

    //So when the center mushroom calls ActivateOnDoubleJump, each mushroom will exeute the LightUp function.
    //You can create new event variables and new activation functions. Just copy everything above and change the name
    //OnDoubleJump and ondoublejump to the things you wanna call them.
    //Good luck!
}
