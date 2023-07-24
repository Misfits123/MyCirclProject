using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton3 : MonoBehaviour {

    public void ClickButton()
    {
        // if button is active
        if (CommonValues.bSceneInfoArray[3] == true && CommonValues.bPopUpActive == false)
        {
            // load new scene
            SceneManager.LoadScene(CommonValues.sSceneArray[1]);
        }
    }
}
