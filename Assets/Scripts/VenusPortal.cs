using UnityEngine;
using System.Collections;

public class VenusPortal : MonoBehaviour {

    public GameObject TeleTarget;
    public GameObject Player;

    public float timing = 1.0f;

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

        if (count >= timing)
        {
            //if (coling.tag == "Player")
            //{
            count = 0;
            onIt = false;
            Player.transform.position = TeleTarget.transform.position;
            Debug.Log("Fuck this!");
            Debug.Log(count);
        }
        //Debug.Log(Time.deltaTime);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            if (onIt == false) onIt = true;

            //if (onIt == true)
            //{
            //onIt = true;
            //}

            Debug.Log("Collisionisms!");
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
