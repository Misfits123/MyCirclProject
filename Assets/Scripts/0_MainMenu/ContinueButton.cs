using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour {

    void Awake()
    {
        // minimum one session must have been played
        if (CommonValues.iNumberOfRounds >= 1 && CommonValues.iNumberOfRounds < CommonValues.iMaxNumberOfRounds)
        {
            // set button active
            gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            // set button inactive
            gameObject.GetComponent<Button>().interactable = false;
        }
    }

    public void ClickButton()
    {
        // if button is active
        if (CommonValues.bPopUpActive == false)
        {
            // load new scene
            CommonValues.sLastScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(CommonValues.sSceneArray[6]);
        }
    }
}
