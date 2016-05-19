using UnityEngine;
using System.Collections;

public class BackOption : MenuOption {

    public override void ExecuteOption()
    {
        GetComponentInParent<StartMenu>().SetSuperLock(false);
        GetComponentInParent<OptionMenu>().enabled = false;
    }
}
