using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartMenu : MonoBehaviour {

    public MenuOption[] options;

    private int selection = 0;

    private int Axis;
    private bool Lock = false;

	// Use this for initialization
	void Start () {
        ChangeSelection(0);
	}

    void OnEnable()
    {
        foreach(MenuOption op in options)
        {
            op.GetComponent<Text>().color = Color.black;
        }
        selection = 0;
        ChangeSelection(0);
        Time.timeScale = 0;
    }
    void OnDisable()
    {
        Time.timeScale = 1;
    }
	
	// Update is called once per frame
	void Update () {

        HandleAxisInput();
        if(!Lock && Axis != 0)
        {
            ChangeSelection(Axis * -1);
            Lock = true;
        }

        if(Input.GetButtonDown("A Button1"))
        {
            options[selection].ExecuteOption();
        }
	}

    private void ChangeSelection(int number)
    {
        options[selection].GetComponent<Text>().color = Color.black;

        selection += number;
        if(selection == options.Length)
        {
            selection = 0;
        }
        else if(selection == -1)
        {
            selection = options.Length - 1;
        }

        options[selection].GetComponent<Text>().color = Color.red;
    }
    private void HandleAxisInput()
    {
        Axis = (int)Input.GetAxisRaw("Left Stick Y1");

        if(Axis == 0)
        {
            Lock = false;
        }
    }
}
