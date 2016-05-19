using UnityEngine;
using System.Collections;

public class TrailerCamera : MonoBehaviour {

    public Transform start;
    public Transform end;
    public ParticleSystem particle;
    public float speed = 10;

    private Transform[] EnableList;

    private bool RunCutscene = false;
    private bool CutsceneDone = false;
    private bool ParticleActive = false;

	// Use this for initialization
	void Start () {
        transform.position = start.position;
	}
	
	// Update is called once per frame
	void Update () {
        if(RunCutscene)
        {
            transform.position = Vector3.MoveTowards(transform.position, end.position, Time.deltaTime * speed);
        }

        if (Vector3.Distance(transform.position, end.position) < 4 && !ParticleActive)
        {
            particle.gameObject.SetActive(true);
        }
        if(transform.position == end.position)
        {
            foreach(Transform t in EnableList)
            {
                t.gameObject.SetActive(true);
            }
            gameObject.SetActive(false);
        }
        //else if (Input.GetButtonDown("X Button"))
        //{
        //    particle.gameObject.SetActive(false);
        //    transform.position = start.position;
        //}
	}

    public void Activate(Transform[] list)
    {
        if(!RunCutscene && !CutsceneDone)
        {

            //disable players and camera
            foreach (Transform t in list)
            {
                t.gameObject.SetActive(false);
            }

            EnableList = list;
            RunCutscene = true;
        }
    }
}
