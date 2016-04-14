using UnityEngine;
using System.Collections;

public class EventSystem : MonoBehaviour {

    public delegate void OnDoubleJump();
    public static event OnDoubleJump ondoublejump;

    public static void ActivateOnDoubleJump()
    {
        if (ondoublejump != null)
            ondoublejump();
    }
}
