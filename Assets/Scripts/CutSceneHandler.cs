using UnityEngine;
using System.Collections;

public class CutSceneHandler : MonoBehaviour {

    public Transform CutsceneCamera;
    public Transform[] DisableList;

    public void ActivateCutscene(int number)
    {
        if(number == 0)
        {
            //turn on cutscene camera
            CutsceneCamera.gameObject.SetActive(true);
            CutsceneCamera.GetComponent<TrailerCamera>().Activate(DisableList);
        }
    }
}
