using UnityEngine;
using System.Collections;

public class PrizeTrigger : MonoBehaviour {

    public GameObject objective;
    public Score scorer;

    public int prize = 1;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            scorer.GivePrize(prize);
            Destroy(objective.gameObject);
            Destroy(this.gameObject);
        }
    }
}
