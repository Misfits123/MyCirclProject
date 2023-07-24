using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectRoundsButton : MonoBehaviour {
    // public variables
    // ----------------------------------------------------------------------------------------------------------------------------
    public GameObject[] roundsArray;

    // privat variables
    // ----------------------------------------------------------------------------------------------------------------------------
    int selectCounter = 1;
    Image buttonImage;
    GameObject okButton;

    void Awake()
    {
        // reference to ok button
        okButton = GameObject.Find("OkButton");

        selectCounter = ((int)CommonValues.iMaxNumberOfRounds / 3) - 1;
        buttonImage = gameObject.GetComponent<Image>();
        buttonImage.sprite = roundsArray[selectCounter].gameObject.GetComponent<Image>().sprite;

        if(CommonValues.bSceneInfoArray[4] == true && CommonValues.bPopUpActive == false)
        {
            selectCounter = 0;
        }
    }

    // update is called once per frame
    void Update()
    {
        // close info text
        for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
        {
            if (CirCl.GetButtonDown(i, 0, false))
            {
                ChangeNumberOfRounds();
            }
        }
    }

    public void ClickButton()
    {
        ChangeNumberOfRounds();
    }

    private void ChangeNumberOfRounds()
    {
        if(CommonValues.bSceneInfoArray[4] == true && CommonValues.bPopUpActive == false)
        {
            selectCounter += 1;
            if (selectCounter > 2)
            {
                selectCounter = 0;
            }
            buttonImage.sprite = roundsArray[selectCounter].gameObject.GetComponent<Image>().sprite;
            CommonValues.iMaxNumberOfRounds = (selectCounter + 1) * 3;
            SetOkButtonActive();
        }
    }
    private void SetOkButtonActive()
    {
        // set button active
        okButton.GetComponent<Button>().interactable = true;
        okButton.GetComponent<Button>().Select();
        okButton.GetComponent<Button>().gameObject.SetActive(true);
    }
}
