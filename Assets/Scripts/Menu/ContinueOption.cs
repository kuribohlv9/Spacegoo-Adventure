using UnityEngine;
using System.Collections;

public class ContinueOption : MenuOption {

    public override void ExecuteOption()
    {
        GetComponentInParent<StartMenu>().gameObject.SetActive(false);
    }
}
