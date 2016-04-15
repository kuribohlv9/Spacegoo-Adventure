using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

    private int score = 0;
    private int prize = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(score);
	}

    public void GiveScore(int gainedScore)
    {
        score += gainedScore;
    }

    public void GivePrize(int gainedPrize)
    {
        prize += gainedPrize;
    }
}
