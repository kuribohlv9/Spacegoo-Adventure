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

    public AudioSource BigPickupSound;
    public AudioSource SmallPickupSound;

    public int ratio = 10;
    public int portalOpeningAt = 10;

    public bool openPortal = false;

    public CutSceneHandler CSH;

    private Text OutputScore;
    private Text OutputPrize;
	
    public void TotalScore(int totalScore)
    {
        scoreTotal += totalScore;
        if (scoreTotal == ratio)
        {
            prizeTotal += 1;
            scoreTotal = 0;

            PrizeText.text = prize.ToString() + "/" + prizeTotal.ToString();
        }
    }

    public void TotalPrize(int totalPrize)
    {
        prizeTotal += totalPrize;

        PrizeText.text = prize.ToString() + "/" + prizeTotal.ToString();
    }

    public void GiveScore(int gainedScore)
    {
        SmallPickupSound.Play();
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
        if (SmallPickupSound.isPlaying)
            SmallPickupSound.Stop();

        BigPickupSound.Play();
        prize += gainedPrize;
        PrizeText.text = prize.ToString() + "/" + prizeTotal.ToString();

        if (prize == portalOpeningAt && !openPortal)
        {
            openPortal = true;
            CSH.ActivateCutscene(0);
        }
    }
}