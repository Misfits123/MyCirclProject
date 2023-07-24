using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton4 : MonoBehaviour {

    public void ClickButton()
    {
        // if button is active
        if (CommonValues.bSceneInfoArray[4] == true && CommonValues.bPopUpActive == false)
        {
            // load new scene
            SceneManager.LoadScene(CommonValues.sLastScene);
        }
    }
}
