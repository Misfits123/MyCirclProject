using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SelectPlayerButton : MonoBehaviour {
    // public variables
    // ----------------------------------------------------------------------------------------------------------------------------
    public GameObject[] oPlayerModeArray;
    
    // privat variables
    // ----------------------------------------------------------------------------------------------------------------------------
    int PlayerMode = 0;                // 0 = Inactive , 1 = Active
    int PlayerIndex = 0;
    Image PlayerButtonImage;
    GameObject OkButton;

    void Awake()
    {
        // set player index
        char[] c_player_button_array = gameObject.name.ToCharArray();
        for (int i = 0; i < c_player_button_array.Length; i++)
        {
            if (Char.IsDigit(c_player_button_array[i]))
            {
                PlayerIndex = c_player_button_array[i] - '0';
            }
        }
        // reference to ok button
        OkButton = GameObject.Find("OkButton");

        // set last player mode
        PlayerButtonImage = gameObject.GetComponent<Image>();
        PlayerButtonImage.sprite = oPlayerModeArray[CommonValues.PlayerParameterArray[PlayerIndex - 1].mode].GetComponent<Image>().sprite;
        PlayerMode = CommonValues.PlayerParameterArray[PlayerIndex - 1].mode;

        // activate ok button
        SetOkButtonActive();
    }

    public void ClickButton()
    {
        // set player mode to player parameter array
        if (PlayerMode == 1 && PlayerIndex - 1 < CommonValues.MaxNumberOfPlayers)
        {
            PlayerMode = 0;
            PlayerButtonImage.sprite = oPlayerModeArray[PlayerMode].GetComponent<Image>().sprite;
            CommonValues.PlayerParameterArray[PlayerIndex - 1].mode = PlayerMode;
        }
        else if (PlayerMode == 0 && PlayerIndex - 1 < CommonValues.MaxNumberOfPlayers)
        {
            PlayerMode = 1;
            PlayerButtonImage.sprite = oPlayerModeArray[PlayerMode].GetComponent<Image>().sprite;
            CommonValues.PlayerParameterArray[PlayerIndex - 1].mode = PlayerMode;
        }
        else
        {
            //do nothing
        }
        // activate ok button
        SetOkButtonActive();
    }

    void Update()
    {
        if (CirCl.GetButtonDown(PlayerIndex - 1, 0))
        {
            // set player mode to player parameter array
            if (PlayerMode == 0 && PlayerIndex - 1 < CommonValues.MaxNumberOfPlayers)
            {
                PlayerMode = 1;
                PlayerButtonImage.sprite = oPlayerModeArray[PlayerMode].GetComponent<Image>().sprite;
                CommonValues.PlayerParameterArray[PlayerIndex - 1].mode = PlayerMode;
            }
            else if (PlayerMode == 1 && PlayerIndex - 1 < CommonValues.MaxNumberOfPlayers)
            {
                PlayerMode = 0;
                PlayerButtonImage.sprite = oPlayerModeArray[PlayerMode].GetComponent<Image>().sprite;
                CommonValues.PlayerParameterArray[PlayerIndex - 1].mode = PlayerMode;
            }
            else
            {
                //do nothing
            }
            // activate ok button
            SetOkButtonActive();
        }
    }

    void SetOkButtonActive()
    {
        // count joined player
        CommonMethods.countNumberOfPlayer();

        // minimum two must be active
        if (CommonValues.NumberOfPlayers > 1)
        {
            // set button active
            OkButton.GetComponent<Button>().interactable = true;
            OkButton.GetComponent<Button>().Select();
            OkButton.GetComponent<Button>().gameObject.SetActive(true);
        }
        else if (CommonValues.NumberOfPlayers <= 1)
        {
            // set button inactive
            OkButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            // do nothing
        }
    }
}
