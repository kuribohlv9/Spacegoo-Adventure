using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RightOption : MonoBehaviour
{
    
    public Transform Sticky;
    public GameObject thisOne;

    public Texture sticky;
    public Texture hoppy;
    public Texture glidy;

    public RawImage img;

    private int which = 0;

    // Use this for initialization
    void Start()
    {
        img = (RawImage)thisOne.GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (target != sourceOfPlayer.target) target = sourceOfPlayer.target;
        if (InsertPlayerInSlime(Sticky) && which != 1)
        {
            img.texture = glidy;
            which = 1;
        }

        if (InsertPlayerInSlime(Sticky.GetComponent<PlayerMachine>().rightSwitchTarget) && which != 2)
        {
            img.texture = hoppy;
            which = 2;
        }

        if (InsertPlayerInSlime(Sticky.GetComponent<PlayerMachine>().rightSwitchTarget.GetComponent<PlayerMachine>().rightSwitchTarget) && which != 3)
        {
            img.texture = sticky;
            which = 3;
        }


    }

    private bool InsertPlayerInSlime(Transform target)
    {
        if (target.GetComponent<PlayerMachine>().InControl)
        {
            return true;
        }
        return false;
    }
}
