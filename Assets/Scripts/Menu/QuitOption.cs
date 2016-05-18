using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class QuitOption : MenuOption {

    public override void ExecuteOption()
    {
        Application.Quit();
    }
}
