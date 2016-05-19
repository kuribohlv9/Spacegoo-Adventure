using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionMenu : MonoBehaviour {
    public MenuOption[] options;

    private int selection = 0;

    private int XAxis;
    private int YAxis;
    private bool Lock = false;

    void OnEnable()
    {
        foreach (MenuOption op in options)
        {
            op.gameObject.SetActive(true);
            op.GetComponent<Text>().color = Color.black;
        }
        selection = 0;
        ChangeSelection(0);
    }

    void OnDisable()
    {
        foreach (MenuOption op in options)
        {
            op.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
            HandleAxisInput();
            if (!Lock && YAxis != 0)
            {
                ChangeSelection(YAxis * -1);
                Lock = true;
            }

            if (!Lock && XAxis != 0)
            {
                Debug.Log("First try");
                options[selection].IncrementOption(XAxis);
                Lock = true;
            }

            if (Input.GetButtonDown("A Button1"))
            {
                options[selection].ExecuteOption();
            }
    }

    private void ChangeSelection(int number)
    {
        options[selection].GetComponent<Text>().color = Color.black;

        selection += number;
        if (selection == options.Length)
        {
            selection = 0;
        }
        else if (selection == -1)
        {
            selection = options.Length - 1;
        }

        options[selection].GetComponent<Text>().color = Color.red;
    }
    private void HandleAxisInput()
    {
        YAxis = (int)Input.GetAxisRaw("Left Stick Y1");
        XAxis = (int)Input.GetAxisRaw("Left Stick X1");

        if (YAxis == 0 && XAxis == 0)
        {
            Lock = false;
        }
    }
}
