using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartOption : MenuOption {
    public override void ExecuteOption()
    {
        SceneManager.LoadScene(1);
    }
}
