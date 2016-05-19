using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LeftOption : MonoBehaviour {

    public PlayerCamera sourceOfPlayer;
    public Transform target;
    public GameObject thisOne;

    public Texture sticky;
    public Texture hoppy;
    public Texture glidy;

    public RawImage img;

    private int which = 0;

    // Use this for initialization
    void Start ()
    {
        //img = (RawImage)thisOne.GetComponent<RawImage>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //if (target != sourceOfPlayer.target) target = sourceOfPlayer.target;
        if (sourceOfPlayer.target.name == "Hoppy_Controller" && which != 1)
        {
            img.texture = glidy;
            which = 1;
        }

        if (sourceOfPlayer.target.name == "Glidy Controller" && which != 2)
        {
            img.texture = sticky;
            which = 2;
        }

        if (sourceOfPlayer.target.name == "Sticky Controller" && which != 3)
        {
            img.texture = glidy;
            which = 3;
        }

        
    }
}
