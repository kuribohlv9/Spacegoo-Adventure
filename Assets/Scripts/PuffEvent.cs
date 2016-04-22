using UnityEngine;
using System.Collections;

public class PuffEvent : MonoBehaviour {

    public GameObject player;

    public float Height = 10;
    public Vector3 direction = new Vector3(0, 1, 0);

    private bool destroy = false;

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void Detonate(bool bang)
    {
        if (bang == true && destroy == false)
        {
            destroy = true;
            player.GetComponentInParent<PlayerMachine>().ChangeMovement(direction * Height);
        }
    }
}
