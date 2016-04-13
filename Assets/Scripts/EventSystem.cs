using UnityEngine;
using System.Collections;

public class EventSystem : MonoBehaviour {

    public delegate void Derp();
    public static event Derp derp;

    public static void ActivateDerp()
    {
        if (derp != null)
            derp();
    }
}
