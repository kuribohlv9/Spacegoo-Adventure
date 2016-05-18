using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraZoomOption : MenuOption {

    public Slider slider;
    public Text numbertext;
    public PlayerCamera camera;
    
    void Start()
    {
        slider.value = camera.Distance - 6;
        numbertext.text = slider.value.ToString();
    }

    public override void IncrementOption(int number)
    {
        slider.value += number;
        camera.Distance = slider.value + 6;
        numbertext.text = slider.value.ToString();
    }
}
