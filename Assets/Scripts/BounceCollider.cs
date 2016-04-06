using UnityEngine;
using System.Collections;

public class BounceCollider : MonoBehaviour {

    AudioSource audio;
    public float Height = 10;
    public Vector3 direction = new Vector3(0, 1, 0);

	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
        direction.Normalize();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            audio.Play();
            col.GetComponentInParent<PlayerMachine>().ChangeMovement(direction * Height);
        }
    }
}
