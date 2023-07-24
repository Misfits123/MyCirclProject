using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour {

    public void ClickButton()
    {
        // if button is active
        if (CommonValues.bSceneInfoArray[6] == true && CommonValues.bPopUpActive == false)
        {
            // load new scene
            SceneManager.LoadScene(CommonValues.sSceneArray[0]);
        }
    }
}
