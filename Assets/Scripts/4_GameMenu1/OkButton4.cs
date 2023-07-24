using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OkButton4 : MonoBehaviour {

    public void ClickButton()
    {
        // if button is active
        if (CommonValues.bSceneInfoArray[4] == true && CommonValues.bPopUpActive == false)
        {
            //CommonValues.iMaxNumberOfRounds = 3;
            SceneManager.LoadScene(CommonValues.sSceneArray[5]);
        }
    }
}
