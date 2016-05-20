using UnityEngine;
using System.Collections;

public class HelpTextArea : MonoBehaviour {

    public GameObject HelpText;

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            HelpText.SetActive(true);
        }
    }
}
