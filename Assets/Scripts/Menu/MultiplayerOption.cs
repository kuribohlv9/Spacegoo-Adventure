using UnityEngine;
using System.Collections;

public class MultiplayerOption : MenuOption {

    public MultiplayerEnabler mpe;

    public override void ExecuteOption()
    {
        if(mpe)
        {
            if (!mpe.IsMultiplayerEnabled)
                mpe.EnableMultiplayer();
            else if(mpe.IsMultiplayerEnabled)
                mpe.DisableMultiplayer();

            GetComponentInParent<StartMenu>().gameObject.SetActive(false);
        }
    }
}
