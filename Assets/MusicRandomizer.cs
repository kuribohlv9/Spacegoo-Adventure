using UnityEngine;
using System.Collections;


//NOTE: THIS MUST BE ATTACHED TO THE CAMERA

public class MusicRandomizer : MonoBehaviour {

	public AudioClip[] soundtrack;

	// Use this for initialization
	void Start () {
        int random = Random.Range(0, soundtrack.Length - 1);

        GetComponent<AudioSource>().clip = soundtrack[random];
        GetComponent<AudioSource>().Play();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (!GetComponent<AudioSource>().isPlaying)
		{
			GetComponent<AudioSource>().clip = soundtrack[Random.Range(0, soundtrack.Length - 1)];
			GetComponent<AudioSource>().Play();
		}
	}
}

