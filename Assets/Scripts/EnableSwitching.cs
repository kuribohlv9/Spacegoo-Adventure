using UnityEngine;
using System.Collections;

public class EnableSwitching : MonoBehaviour {

    private bool HasTriggered = false;
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            col.GetComponent<PlayerMachine>().EnableSwitching = true;
        }
    }
}
