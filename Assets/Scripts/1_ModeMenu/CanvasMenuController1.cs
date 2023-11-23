using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasMenuController1 : MonoBehaviour
{
    // update is called once per frame
    void Update()
    {
        if(CommonValues.bPopUpActive == false)
        {
            for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
            {
                if (CirCl.GetButtonDown(i, 0))
                {
                    // load new scene
                    SceneManager.LoadScene(CommonValues.sSceneArray[0]);
                }
            }
            CirCl.SelectUiButton(CommonValues.iEventSystemPlayer);
        }
    }
}
