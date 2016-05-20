using UnityEngine;
using System.Collections;

public class DisableTimer : MonoBehaviour {

    public float timer = 1;
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            if (GetComponent<ParticleSystem>().isPlaying)
                GetComponent<ParticleSystem>().Stop();
            else
                gameObject.SetActive(false);

            timer = 1;
        }
	}
}
