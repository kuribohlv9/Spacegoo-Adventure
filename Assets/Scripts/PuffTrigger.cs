using UnityEngine;
using System.Collections;

public class PuffTrigger : MonoBehaviour {


    public PuffEvent Puffer;

    public float timing = 0.3f;

    private float count = 0.0f;
    private bool onIt = false;
    private bool explode = false;

    // Use this for initialization
    void Start ()
    {
	
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
            count = 0;
            onIt = false;
            explode = true;
            Puffer.Detonate(explode);
            Debug.Log(count);
        }

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            if (onIt == false) onIt = true;
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
