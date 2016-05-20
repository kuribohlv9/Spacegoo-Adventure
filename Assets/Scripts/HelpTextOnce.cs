using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HelpTextOnce : MonoBehaviour {

    public Text[] helptext;
    public Image[] helpimage;
    public float fadeinspeed = 1;
    public float fadeoutspeed = 1;

    private bool fadein = false;
    private bool fadeout = false;
    private bool havebeenplayed = false;
    private Color CurrentColor;

    void Start()
    {
        CurrentColor.a = 0;
    }

    void OnEnable()
    {
        if (havebeenplayed)
            gameObject.SetActive(false);

        fadein = true;
        fadeout = false;

        CurrentColor.a = 0;
        
        foreach(Text t in helptext)
        {
            t.color = CurrentColor;
        }

        foreach (Image i in helpimage)
        {
            i.color = CurrentColor;
        }
    }
    void OnDisable()
    {
        fadein = false;
        fadeout = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (fadein)
        {
            CurrentColor.a = Mathf.MoveTowards(CurrentColor.a, 1, Time.deltaTime * fadeinspeed);

            foreach (Text t in helptext)
            {
                t.color = CurrentColor;
            }

            foreach (Image i in helpimage)
            {
                i.color = CurrentColor;
            }

            if (CurrentColor.a == 1)
            {
                fadein = false;
            }
        }
        else if (fadeout)
        {
            CurrentColor.a = Mathf.MoveTowards(CurrentColor.a, 0, Time.deltaTime * fadeoutspeed);

            foreach (Text t in helptext)
            {
                t.color = CurrentColor;
            }

            foreach (Image i in helpimage)
            {
                i.color = CurrentColor;
            }

            if (CurrentColor.a == 0)
            {
                gameObject.SetActive(false);
                havebeenplayed = true;
            }
        }
        if(Input.GetButtonDown("B Button1"))
        {
            fadeout = true;
        }
	}
}
