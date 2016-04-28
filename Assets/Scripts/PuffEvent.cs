using UnityEngine;
using System.Collections;

public class PuffEvent : MonoBehaviour {

    public GameObject player;
    public GameObject thisCollider;

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
        direction = player.transform.position - this.transform.position;
        //direction *= -1;
        direction.Normalize();

        if (destroy == true)
        {
            Destroy(thisCollider);
            Destroy(this);
        }
    }

    public void Detonate(Collider bang)
    {
        if (destroy == false)
        {
            destroy = true;
            //bang.GetComponentInParent<PlayerMachine>().currentState = PlayerStates;
            bang.GetComponentInParent<PlayerMachine>().ChangeMovement(direction * Height);
        }
    }
}
