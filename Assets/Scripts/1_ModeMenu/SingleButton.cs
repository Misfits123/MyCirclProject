using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleButton : MonoBehaviour {

    public void ClickButton()
    {
        if(CommonValues.bPopUpActive == false)
        {
            // set game mode
            CommonValues.GameMode = 1;

            // set last difficulty level
            CommonValues.iDifficultyLevel = 0;

            // set player mode zero
            for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
            {
                CommonValues.PlayerParameterArray[i].mode = 0;
            }

            // set number of rounds back
            CommonValues.iMaxNumberOfRounds = 6;

            // load new scene
            SceneManager.LoadScene(CommonValues.sSceneArray[2]);
        }
    }
}
