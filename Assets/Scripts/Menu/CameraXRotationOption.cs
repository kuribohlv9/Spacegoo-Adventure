using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraXRotationOption : MenuOption
{
    public Slider slider;
    public Text numbertext;
    public PlayerCamera camera;

    void Start()
    {
        slider.value = camera.xRotationSpeed / 10;
        numbertext.text = slider.value.ToString();
    }

    public override void IncrementOption(int number)
    {
        slider.value += number;
        camera.xRotationSpeed = slider.value * 10;
        numbertext.text = slider.value.ToString();
    }
}
