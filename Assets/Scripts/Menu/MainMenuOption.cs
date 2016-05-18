using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuOption : MenuOption
{
    public Image loadingscreen;

    public override void ExecuteOption()
    {
        loadingscreen.gameObject.SetActive(true);
        SceneManager.LoadScene(0);
    }
}
