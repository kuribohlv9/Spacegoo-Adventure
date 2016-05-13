using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    private int score = 0;
    private int scoreTotal = 0;
    private int prize = 0;
    private int prizeTotal = 0;

    public Text ScoreText;
    public Text PrizeText;


    //public Text ScoreTotalText;
    //public Text PrizeTotalText;

    private Text OutputScore;
    private Text OutputPrize;

    // Use this for initialization
    void Start ()
    {
        ScoreText.text = score.ToString() + "/" + scoreTotal.ToString();
        PrizeText.text = prize.ToString() + "/" + prizeTotal.ToString();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Debug.Log(score);
        

    }

    public void TotalScore(int totalScore)
    {
        scoreTotal += totalScore;
        //ScoreTotalText.text = scoreTotal.ToString();
    }

    public void TotalPrize(int totalPrize)
    {
        prizeTotal += totalPrize;
        //PrizeTotalText.text = prizeTotal.ToString();
    }

    public void GiveScore(int gainedScore)
    {
        score += gainedScore;
        ScoreText.text = score.ToString() + "/" + scoreTotal.ToString();

    }

    public void GivePrize(int gainedPrize)
    {
        prize += gainedPrize;
        PrizeText.text = prize.ToString() + "/" + prizeTotal.ToString();
    }
}
