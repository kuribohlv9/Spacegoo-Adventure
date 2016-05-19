using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelOption : MenuOption {

    public Slider slider;
    public Text numbertext;
    public PlayerCamera camera;
    
    void Start()
    {
        numbertext.text = slider.value.ToString();
    }

    public override void IncrementOption(int number)
    {
        slider.value += number;
        numbertext.text = slider.value.ToString();
    }
}
