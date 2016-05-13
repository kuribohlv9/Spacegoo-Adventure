using UnityEngine;
using System.Collections;

public class ShrivelTrigger : MonoBehaviour {

    private bool shrink = false;
    private bool undone = false;
    private bool grow = false;

    private float xOriginal;
    private float yOriginal;

    private float timer = 0;

    public float desizeSpeed = 0.1f;
    public float resizeSpeed = 0.05f;
    public float growthDelay = 3.0f;

    public GameObject target1;
    public MeshCollider target2;

	AudioSource audio;

    // Use this for initialization
    void Start ()
    {
        xOriginal = target1.transform.localScale.x;
        yOriginal = target1.transform.localScale.y;

		audio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (shrink == true)
        {
            if (target1.transform.localScale.x >= xOriginal/10)
            {
                target1.transform.localScale += new Vector3(-desizeSpeed, 0, 0);
            }

            if (target1.transform.localScale.y >= yOriginal / 10)
            {
                target1.transform.localScale += new Vector3(0, -desizeSpeed, 0);
            }


        }
	
        if (undone == true && target1.transform.localScale.x < xOriginal)
        {
            timer += Time.deltaTime;

            if (timer >= growthDelay)
            {
                shrink = false;
                grow = true;
                timer = 0;
                undone = false;
            }
        }

        if (grow == true)
        {
            if (target1.transform.localScale.x < xOriginal)
            {
                target1.transform.localScale += new Vector3(resizeSpeed, 0, 0);
            }

            if (target1.transform.localScale.y < yOriginal)
            {
                target1.transform.localScale += new Vector3(0, resizeSpeed, 0);
            }


        }

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
			if (!audio.isPlaying) 
			{
				if (undone == false) 
				{
					audio.Play();
				}
			}

			shrink = true;
            undone = false;
            grow = false;
            timer = 0;

        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            undone = true;
            
        }
    }
}
