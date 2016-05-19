using UnityEngine;
using System.Collections;

public class OptionOption : MenuOption {

    void OnDisable()
    {
        GetComponent<OptionMenu>().enabled = false;
    }

    public override void ExecuteOption()
    {
        GetComponentInParent<StartMenu>().SetSuperLock(true);
        GetComponent<OptionMenu>().enabled = true;
    }
}
