using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButton2 : MonoBehaviour {
    // public variables
    // ----------------------------------------------------------------------------------------------------------------------------

    // privat variables
    // ----------------------------------------------------------------------------------------------------------------------------
    private GameObject MainMenuButton;
    void Awake()
    {
        MainMenuButton = GameObject.Find("MainMenuButton");
    }
    void Update()
    {
        if(CommonValues.iNumberOfRounds >= CommonValues.iMaxNumberOfRounds)
        {
            // set button inactive
            gameObject.GetComponent<Button>().interactable = false;
            MainMenuButton.GetComponent<Button>().interactable = true;
            MainMenuButton.GetComponent<Button>().Select();
            MainMenuButton.GetComponent<Button>().gameObject.SetActive(true);
        }
    }
    public void ClickButton()
    {
        // if button is active
        if (CommonValues.bSceneInfoArray[6] == true && CommonValues.bPopUpActive == false)
        {
            // load new scene
            SceneManager.LoadScene(CommonValues.sSceneArray[5]);
        }
    }
}
