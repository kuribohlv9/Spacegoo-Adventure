﻿using UnityEngine;
using System.Collections;

public class MultiplayerEnabler : MonoBehaviour {

    public Transform OtherCamera;
    public Transform Sticky;

    private bool Multiplayer = false;
    private Camera MainCamera;
    private float width = 1;

	// Use this for initialization
	void Start () {
        MainCamera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(!Multiplayer && Input.GetButtonDown("Debug F3"))
        {
            OtherCamera.gameObject.SetActive(true);
            Multiplayer = true;
            
            if(!InsertPlayerInSlime(Sticky))
            {
                if (!InsertPlayerInSlime(Sticky.GetComponent<PlayerMachine>().rightSwitchTarget))
                {
                    InsertPlayerInSlime(Sticky.GetComponent<PlayerMachine>().rightSwitchTarget.GetComponent<PlayerMachine>().rightSwitchTarget);
                }
            }

            width = 0.5f;
        }
        else if (Multiplayer && Input.GetButtonDown("Debug F3"))
        {
            Multiplayer = false;

            PurifySlimes(Sticky);
            PurifySlimes(Sticky.GetComponent<PlayerMachine>().rightSwitchTarget);
            PurifySlimes(Sticky.GetComponent<PlayerMachine>().rightSwitchTarget.GetComponent<PlayerMachine>().rightSwitchTarget);

            width = 1.0f;
        }
        if(MainCamera.rect.width != width)
        {
            MainCamera.rect = new Rect(0, 0, Mathf.MoveTowards(MainCamera.rect.width, width, Time.deltaTime), 1);
        }
        if (MainCamera.rect.width == 1.0f)
        {
            OtherCamera.gameObject.SetActive(false);
        }
	}

    private void PurifySlimes(Transform target)
    {
        if(target.GetComponent<PlayerInputController>().PlayerNumber == "2")
        {
            target.GetComponent<PlayerInputController>().PlayerNumber = "1";
            target.GetComponent<PlayerMachine>().InControl = false;
            target.GetComponent<PlayerMachine>().ChangeState(PlayerMachine.PlayerStates.NoControl);
        }
    }
    private bool InsertPlayerInSlime(Transform target)
    {
        if (!target.GetComponent<PlayerMachine>().InControl)
        {
            target.GetComponent<PlayerMachine>().InControl = true;
            target.GetComponent<PlayerInputController>().PlayerNumber = "2";
            target.GetComponent<PlayerMachine>().camera = OtherCamera;
            OtherCamera.GetComponent<PlayerCamera>().SetTarget(target);
            return true;
        }
        return false;
    }
}
