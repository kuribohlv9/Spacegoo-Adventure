using UnityEngine;
using System.Collections;

public class CollectionTrigger : MonoBehaviour {

    public GameObject objective;
    public Score scorer;

    public int score = 10;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            scorer.GiveScore(score);
            Destroy(objective.gameObject);
        }
    }
}
