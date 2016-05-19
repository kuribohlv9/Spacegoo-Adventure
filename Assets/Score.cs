using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    private int score = 0;
    private int scoreTotal = 0;
    private int prize = 0;
    private int prizeTotal = 0;

    private float secondCheck;

    public Text ScoreText;
    public Text PrizeText;

    public int ratio = 10;
    public int portalOpeningAt = 10;

    public bool openPortal = false;

    public CutSceneHandler CSH;


    //public Text ScoreTotalText;
    //public Text PrizeTotalText;

    private Text OutputScore;
    private Text OutputPrize;

    // Use this for initialization
    void Start ()
    {
        //ScoreText.text = score.ToString() + "/" + ratio.ToString();
        //PrizeText.text = prize.ToString() + "/" + prizeTotal.ToString();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //secondCheck += Time.deltaTime;
        //if (secondCheck >= 0.5) PrizeText.text = prize.ToString() + "/" + prizeTotal.ToString();
        // Debug.Log(score);


    }

    public void TotalScore(int totalScore)
    {
        scoreTotal += totalScore;
        if (scoreTotal == ratio)
        {
            prizeTotal += 1;
            scoreTotal = 0;

            PrizeText.text = prize.ToString() + "/" + prizeTotal.ToString();
        }
        //ScoreTotalText.text = scoreTotal.ToString();
    }

    public void TotalPrize(int totalPrize)
    {
        prizeTotal += totalPrize;

        PrizeText.text = prize.ToString() + "/" + prizeTotal.ToString();
        //PrizeTotalText.text = prizeTotal.ToString();
    }

    public void GiveScore(int gainedScore)
    {
        score += gainedScore;
        if (score == ratio)
        {
            score = 0;
            ScoreText.text = score.ToString() + "/" + ratio.ToString();
            GivePrize(1);
        }
        ScoreText.text = score.ToString() + "/" + ratio.ToString();

    }

    public void GivePrize(int gainedPrize)
    {
        prize += gainedPrize;
        PrizeText.text = prize.ToString() + "/" + prizeTotal.ToString();

        if (prize == portalOpeningAt && !openPortal)
        {
            openPortal = true;
            CSH.ActivateCutscene(0);
        }
    }
}