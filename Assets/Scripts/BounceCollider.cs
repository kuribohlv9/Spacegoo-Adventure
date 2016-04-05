using UnityEngine;
using System.Collections;

public class BounceCollider : MonoBehaviour {

    AudioSource audio;
    public float Height = 10;

	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            audio.Play();
            col.GetComponentInParent<PlayerMachine>().ChangeMovement(Vector3.up * Height);
        }
    }
}
