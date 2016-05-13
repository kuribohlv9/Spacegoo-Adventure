using UnityEngine;
using System.Collections;

public class VenusPortal : MonoBehaviour {

    public GameObject TeleTarget;
    public float timing = 0.3f;

    private float count = 0.0f;
    private Animator anim;

    // Use this for initialization
    void Start () {
        anim = GetComponentInParent<Animator>();
	}

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            count += Time.deltaTime;
            //anim.SetFloat("Count", count);

            if (count >= timing)
            {
                col.transform.parent.transform.position = TeleTarget.transform.position;
                count = 0;
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            count = 0;
        }
    }

}
