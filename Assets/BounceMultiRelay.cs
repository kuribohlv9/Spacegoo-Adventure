using UnityEngine;
using System.Collections;

public class BounceMultiRelay : MonoBehaviour
{
    public float timeLimit;
    public float shrink = 0.1f;

    public bool triggerHere = false; // Works as the trigger that turns the targets targeting this active.

    private float time = 0.0f;
    private bool timerOn = false;

    private float sizeUp;

    // Use this for initialization
    void Start ()
    {
          
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (timerOn == true)
        {
            time += Time.deltaTime;
            if (gameObject.transform.localScale.z >= sizeUp / 2) transform.localScale += new Vector3(0, 0, -shrink);

            //gameObject.transform.localScale(gameObject.transform.localScale.x, gameObject.transform.localScale.y-shrink, gameObject.transform.localScale.z);
        }

        else if (gameObject.transform.localScale.z <= sizeUp)
        {
            transform.localScale += new Vector3(0, 0, shrink);
            //gameObject.transform.localScale(gameObject.transform.localScale.x, gameObject.transform.localScale.y+shrink, gameObject.transform.localScale.z);
        }

        else if (gameObject.transform.localScale.z >= sizeUp)
        {
            //transform.localScal
            //gameObject.transform.localScale(gameObject.transform.localScale.x, sizeUp, gameObject.transform.localScale.z);
        }

        if (time >= timeLimit)
        {
            timerOn = false;
            time = 0;
            triggerHere = false;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            triggerHere = true;
            timerOn = true;
        }
    }
}
