using UnityEngine;
using System.Collections;

public class ShadowBehaviour : MonoBehaviour {

    public Transform target;
    public Vector3 offset;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = target.position + offset;
	}
}
