using UnityEngine;
using System.Collections;

public class TrailerCamera : MonoBehaviour {

    public Transform start;
    public Transform end;
    public float speed = 10;
    public Transform particle;

	// Use this for initialization
	void Start () {
        transform.position = start.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, end.position, Time.deltaTime * speed);

        if (Input.GetButtonDown("A Button"))
        {
            particle.gameObject.SetActive(true);
        }
        else if (Input.GetButtonDown("X Button"))
        {
            particle.gameObject.SetActive(false);
            transform.position = start.position;
        }
	}
}
