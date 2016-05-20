using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlsOption : MenuOption {

    public Image controlimage;

    void OnDisable()
    {
        controlimage.gameObject.SetActive(false);
    }

    public override void ExecuteOption()
    {
        GetComponentInParent<StartMenu>().SetSuperLock(true);
        controlimage.gameObject.SetActive(true);
    }
}
