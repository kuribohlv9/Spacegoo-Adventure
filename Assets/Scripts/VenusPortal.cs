using UnityEngine;
using System.Collections;

public class VenusPortal : MonoBehaviour {

    public GameObject TeleTarget;
    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;

    public float timing = 0.3f;

    private int whichPlayer = 0;
    private float count = 0.0f;
    private bool onIt = false;

    Collider coling;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (onIt == true)
        {
            count += Time.deltaTime;
            Debug.Log(count);
        }

        
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            if (onIt == false) onIt = true;
            /*if (col == Player1) whichPlayer = 1;
            if (col == Player1) whichPlayer = 2;
            if (col == Player1) whichPlayer = 1;*/

            if (count >= timing)
            {
                count = 0;
                onIt = false;
                //col.transform.position = TeleTarget.transform.position;
                col.transform.parent.transform.position = TeleTarget.transform.position;
                Debug.Log(count);
            }
        }

        
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            onIt = false;
            count = 0;
        }
    }

}
