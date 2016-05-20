using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartMenu : MonoBehaviour {

    public MenuOption[] options;
    public AudioSource selectionsound;

    private int selection = 0;
    private int Axis;
    private bool Lock = false;
    private bool SuperLock = false;

	// Use this for initialization
	void Start () {
        ChangeSelection(0);
	}

    void OnEnable()
    {
        foreach(MenuOption op in options)
        {
            Color temp = op.GetComponent<Image>().color;
            temp.a = 0.5f;
            op.GetComponent<Image>().color = temp;
        }
        selection = 0;
        ChangeSelection(0);
        Time.timeScale = 0;
        SuperLock = false;
    }
    void OnDisable()
    {
        Time.timeScale = 1;
    }
	
	// Update is called once per frame
	void Update () {
        if(!SuperLock)
        {
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
	}

    private void ChangeSelection(int number)
    {
        selectionsound.Play();

        Color temp = options[selection].GetComponent<Image>().color;
        temp.a = 0.5f;
        options[selection].GetComponent<Image>().color = temp;

        selection += number;
        if(selection == options.Length)
        {
            selection = 0;
        }
        else if(selection == -1)
        {
            selection = options.Length - 1;
        }

        temp = options[selection].GetComponent<Image>().color;
        temp.a = 1f;
        options[selection].GetComponent<Image>().color = temp;
    }
    private void HandleAxisInput()
    {
        Axis = (int)Input.GetAxisRaw("Left Stick Y1");

        if(Axis == 0)
        {
            Lock = false;
        }
    }
    public void SetSuperLock(bool state)
    {
        SuperLock = state;
    }
}
