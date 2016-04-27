using UnityEngine;
using System.Collections;

public class PuffTrigger : MonoBehaviour {


    public PuffEvent Puffer;

    public float timing = 0.3f;

    private float count = 0.0f;
    private bool onIt = false;
    public bool explode = false;

    private Collider ColD;

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
            
            Debug.Log(count);
        }

        if (ColD.tag == "Player")
        {
            if (explode == true)
            {
                Puffer.Detonate(ColD);
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            if (onIt == false) onIt = true;
            ColD = col;
        }

       
    }

    /*void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            if (explode == true)
            {
                Puffer.Detonate(col);
            }
        }
    }*/

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            onIt = false;
            count = 0;
        }
    }
}
