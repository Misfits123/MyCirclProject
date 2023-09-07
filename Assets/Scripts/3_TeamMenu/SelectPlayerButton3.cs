using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SelectPlayerButton3 : MonoBehaviour {
    // public variables
    // ----------------------------------------------------------------------------------------------------------------------------
    public GameObject[] oPlayerModeArray;
    
    // privat variables
    // ----------------------------------------------------------------------------------------------------------------------------
    int PlayerMode = 0;
    int PlayerTeam = 0;
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
        PlayerButtonImage.sprite = oPlayerModeArray[CommonValues.PlayerParameterArray[PlayerIndex - 1].team].GetComponent<Image>().sprite;
        PlayerMode = CommonValues.PlayerParameterArray[PlayerIndex - 1].mode;
        PlayerTeam = CommonValues.PlayerParameterArray[PlayerIndex - 1].team;

        // activate ok button
        SetOkButtonActive();
    }

    public void ClickButton()
    {
        // set player mode to player parameter array
        if (PlayerTeam == oPlayerModeArray.Length - 1)
        {
            PlayerMode = 0;
            PlayerTeam = 0;
        }
        else
        {
            PlayerMode = 1;
            PlayerTeam += 1;
        }
        PlayerButtonImage.sprite = oPlayerModeArray[PlayerTeam].GetComponent<Image>().sprite;
        CommonValues.PlayerParameterArray[PlayerIndex - 1].mode = PlayerMode;
        CommonValues.PlayerParameterArray[PlayerIndex - 1].team = PlayerTeam;

        // activate ok button
        SetOkButtonActive();
    }

    void Update()
    {
        if (CirCl.GetButtonDown(PlayerIndex - 1, 0))
        {
            // set player mode to player parameter array
            if (PlayerTeam == oPlayerModeArray.Length - 1)
            {
                PlayerMode = 0;
                PlayerTeam = 0;
            }
            else
            {
                PlayerMode = 1;
                PlayerTeam += 1;
            }

            PlayerButtonImage.sprite = oPlayerModeArray[PlayerTeam].GetComponent<Image>().sprite;
            CommonValues.PlayerParameterArray[PlayerIndex - 1].mode = PlayerMode;
            CommonValues.PlayerParameterArray[PlayerIndex - 1].team = PlayerTeam;

            // activate ok button
            SetOkButtonActive();
        }
    }

    void SetOkButtonActive()
    {
        // count joined player
        CommonMethods.countNumberOfPlayer();

        // count number of teams
        CommonMethods.countNumberOfTeams();

        // two different teams
        if (CommonValues.NumberOfTeams > 1)
        {
            // set button active
            OkButton.GetComponent<Button>().interactable = true;
            OkButton.GetComponent<Button>().Select();
            OkButton.GetComponent<Button>().gameObject.SetActive(true);
        }
        else if (CommonValues.NumberOfTeams <= 1)
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
