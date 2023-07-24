using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class OkButton2 : MonoBehaviour {

    public void ClickButton()
    {
        // if button is active
        if (CommonValues.bSceneInfoArray[2] == true && CommonValues.bPopUpActive == false)
        {
            // set player level
            if (CommonValues.GameLevel == 0)
            {
                // set easy
                CommonValues.MaxNumberOfItems = 0;
                CommonValues.MaxNumberOfBalls = 1;
            }
            else if (CommonValues.GameLevel == 1)
            {
                // set medium
                CommonValues.MaxNumberOfItems = 2;
                CommonValues.MaxNumberOfBalls = 1;
            }
            else
            {
                //set hard
                CommonValues.MaxNumberOfItems = 3;
                CommonValues.MaxNumberOfBalls = 3;
            }

            // minimum two must be active
            if (CommonValues.NumberOfPlayers > 1)
            {
                // set last scene
                CommonValues.sLastScene = SceneManager.GetActiveScene().name;

                // load new scene
                SceneManager.LoadScene(CommonValues.sSceneArray[4]);
            }
        }
    }
}
