using UnityEngine;
using System.Collections;

public class BounceRelay : MonoBehaviour {

    public BounceTarget Target;

    public float timeLimit;
    public float shrink = 0.1f;

    private float time = 0.0f;
    private bool timerOn = false;

    private float sizeUp;



	// Use this for initialization
	void Start ()
    {
        sizeUp = gameObject.transform.localScale.y;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (timerOn == true)
        {
            time += Time.deltaTime;
            if (gameObject.transform.localScale.z >= sizeUp/2) transform.localScale += new Vector3(0, 0, -shrink);

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
            Target.remoteTriggered = false;
        }
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            Target.remoteTriggered = true;
            timerOn = true;
        }
    }
}
