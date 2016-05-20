using UnityEngine;
using System.Collections;

public class EndScreenFade : MonoBehaviour {

    public float timer;
    public UnityEngine.UI.Image black;
    public float speed = 1;
    public AudioSource audio;
    public EndScreenScene screen;

    private Color alpha;

	// Use this for initialization
	void Start () {
        alpha = black.color;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
	    if(timer < 0)
        {
            if (!audio.isPlaying)
                audio.Play();

            alpha.a = Mathf.MoveTowards(alpha.a, 0,  Time.deltaTime * speed);
            black.color = alpha;
            if(alpha.a == 0)
            {
                screen.startfade = true;
                gameObject.SetActive(false);
            }
        }
	}
}
