using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CanvasMenuController : MonoBehaviour
{
    // public static variables
    // ----------------------------------------------------------------------------------------------------------------------------
    // public variables
    // ----------------------------------------------------------------------------------------------------------------------------
    public GameObject[] oCanvasObjectArray;
    public GameObject[] oCanvasObjectGermanArray;
    public Image mask;

    // private variables
    // ----------------------------------------------------------------------------------------------------------------------------
    private GameObject audioManagerObj;
    private AudioManager audioManagerComp;
    private Text gameMusicText;
    private Text gameInfoText;
    //private Text gamePlayerText;
    //private GameObject gamePlayer;
    private GameObject popUpPlayerAssignment;
    private GameObject maskPopUpPlayerAssignment;
    private bool isCoroutineStarted = false;
    private bool isMenuMusicStarted = false;
    private int iPopUpStart = 6;
    private int iPopUpCounter = 6;
    private float debounceButtonTime = 2f;
    //private Text debug;

    void Awake()
    {
        // set references
        audioManagerObj = GameObject.Find("AudioManager");
        audioManagerComp = audioManagerObj.GetComponent<AudioManager>();
        gameMusicText = GameObject.Find("GameMusic").GetComponent<Text>();
        gameInfoText = GameObject.Find("GameInfo").GetComponent<Text>();
        //gamePlayerText = GameObject.Find("GamePlayer").GetComponent<Text>();
        //gamePlayer = GameObject.Find("GamePlayer");
        popUpPlayerAssignment = GameObject.Find("CanvasSceneless").transform.Find("PopUpPlayerAssignment").gameObject;
        maskPopUpPlayerAssignment = popUpPlayerAssignment.transform.Find("Mask").gameObject;

        // load saved settings before game starts
        GetSavedSettings();

        // show menu infos just once
        if (CommonValues.bSceneInfoArray[0] == true)
        {
            for (int j = 0; j < oCanvasObjectArray.Length; j++)
            {
                oCanvasObjectArray[j].gameObject.SetActive(false);
            }
            mask.GetComponent<Animator>().SetTrigger("ClearMask");
        }
        else
        {
            CommonValues.bPopUpActive = true;
            oCanvasObjectArray[iPopUpStart].gameObject.SetActive(true);
        }
    }

    void Start()
    {
        audioManagerComp.stopGameMenuMusic2();
        if (CommonValues.bSceneInfoArray[0] == false)
        {
            audioManagerComp.playGameInfoApear();
        }
    }

    void Update()
    {
        if (CommonValues.bPopUpActive == true)
        {
            // close info text
            for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
            {
                if (CirCl.GetButtonDown(i, 0) && iPopUpCounter == 0)
                {
                    // start
                    if (isCoroutineStarted == false)
                    {
                        isCoroutineStarted = true;
                        CommonValues.iEventSystemPlayer = i;
                        StartCoroutine(delayedPopUpChange());
                    }
                }
            }
        }
        if(CommonValues.bPopUpActive == false && isMenuMusicStarted == false)
        {
            isMenuMusicStarted = true;
            audioManagerComp.startGameMenuMusic();
        }
        if(isMenuMusicStarted == true && debounceButtonTime > 0f)
        {
            debounceButtonTime -= Time.deltaTime;
        }
        if(debounceButtonTime <= 0f)
        {
            CirCl.SelectUiButton(CommonValues.iEventSystemPlayer);
        }
    }

    public void ClickCirclLogoButton()
    {
        if (CommonValues.bPopUpActive == true)
        {
            // start
            if (isCoroutineStarted == false)
            {
                isCoroutineStarted = true;
                StartCoroutine(delayedPopUpChange());
            }
        }
    }

    public void ClickWithoutCirclButton()
    {
        if(isCoroutineStarted == false)
        {
            PlayerPrefs.SetInt("PlayWithoutCircl", 1);

            oCanvasObjectArray[iPopUpCounter].gameObject.SetActive(false);
            oCanvasObjectGermanArray[iPopUpCounter].gameObject.SetActive(false);
            CommonValues.bSceneInfoArray[0] = true;

            // load saved language settings
            GetSavedSettings();

            // show player assignment
            mask.GetComponent<Animator>().SetTrigger("ClearMask");
            audioManagerComp.playMaskDisapear();

            SetNewGameButtonActive();
            CommonValues.bPopUpActive = false;
            isCoroutineStarted = false;
        }
    }

    public void ClickBritishFlagButton()
    {
        if (CommonValues.bPopUpActive == true)
        {
            // start
            if (isCoroutineStarted == false)
            {
                isCoroutineStarted = true;
                CommonValues.bGermanLanguage = false;
                PlayerPrefs.SetInt("GermanLanguage", 0);
                StartCoroutine(delayedPopUpChange());
            }
        }
    }

    public void ClickGermanFlagButton()
    {
        if (CommonValues.bPopUpActive == true)
        {
            // start
            if (isCoroutineStarted == false)
            {
                CommonValues.bGermanLanguage = true;
                PlayerPrefs.SetInt("GermanLanguage", 1);
                oCanvasObjectArray[iPopUpCounter].gameObject.SetActive(false);
                oCanvasObjectGermanArray[iPopUpCounter - 1].gameObject.SetActive(true);
                audioManagerComp.playMaskDisapear();
                iPopUpCounter -= 1;
            }
        }
    }

    public void ClickMusicButton()
    {
        // if button is active
        if (CommonValues.bPopUpActive == false)
        {
            if (CommonValues.bMusicDeactive == false)
            {
                if (CommonValues.bGermanLanguage == true)
                {
                    gameMusicText.text = "MUSIK: AUS";
                }
                else
                {
                    gameMusicText.text = "MUSIC: OFF";
                }
                CommonValues.bMusicDeactive = true;
                PlayerPrefs.SetInt("MusicDeactive", 1);
                audioManagerComp.stopGameMenuMusic();
            }
            else
            {
                if (CommonValues.bGermanLanguage == true)
                {
                    gameMusicText.text = "MUSIK: AN";
                }
                else
                {
                    gameMusicText.text = "MUSIC: ON";
                }
                CommonValues.bMusicDeactive = false;
                PlayerPrefs.SetInt("MusicDeactive", 0);
                audioManagerComp.startGameMenuMusic();
            }
            SetNewGameButtonActive();
        }
    }

    public void ClickInfoButton()
    {
        // if button is active
        if (CommonValues.bPopUpActive == false)
        {
            if (CommonValues.bInfoDeactive == true)
            {
                CommonValues.bInfoDeactive = false;
                PlayerPrefs.SetInt("InfoDeactive", 0);
                if (CommonValues.bGermanLanguage == true)
                {
                    gameInfoText.text = "INFO: AN";
                }
                else
                {
                    gameInfoText.text = "INFO: ON";
                }
                for (int i = 0; i < CommonValues.bSceneInfoArray.Length; i++)
                {
                    CommonValues.bSceneInfoArray[i] = false;
                }
                CommonValues.bPopUpActive = true;
                oCanvasObjectArray[iPopUpStart].gameObject.SetActive(true);
                mask.GetComponent<Animator>().SetTrigger("SetMask");
                iPopUpCounter = iPopUpStart;
            }
            else
            {
                if (CommonValues.bGermanLanguage == true)
                {
                    gameInfoText.text = "INFO: AUS";
                }
                else
                {
                    gameInfoText.text = "INFO: OFF";
                }
                CommonValues.bInfoDeactive = true;
                PlayerPrefs.SetInt("InfoDeactive", 1);
                for (int i = 0; i < CommonValues.bSceneInfoArray.Length; i++)
                {
                    CommonValues.bSceneInfoArray[i] = true;
                }
            }
            SetNewGameButtonActive();
        }
    }

    public void ClickHomepageButton()
    {
        if (CommonValues.bGermanLanguage == true)
        {
            Application.OpenURL("https://www.playinacircle.com/index.html");
        }
        else
        {
            Application.OpenURL("https://www.playinacircle.com/index_en.html");
        }
    }

    private void SetNewGameButtonActive()
    {
        // set button active
        gameObject.transform.Find("NewGameButton").gameObject.GetComponent<Button>().interactable = true;
    }

    private void GetSavedSettings()
    {
        if(PlayerPrefs.GetInt("PlayWithoutCircl") == 1)
        {
            CommonValues.bPlayWithoutCircl = true;
            //gamePlayer.SetActive(false);
        }
        else
        {
            CommonValues.bPlayWithoutCircl = false;
            //gamePlayer.SetActive(true);
        }
        if (PlayerPrefs.GetInt("GermanLanguage") == 0)
        {
            CommonValues.bGermanLanguage = false;
        }
        else
        {
            CommonValues.bGermanLanguage = true;
        }
        if (PlayerPrefs.GetInt("InfoDeactive") == 0)
        {
            CommonValues.bInfoDeactive = false;
            debounceButtonTime = 1f;
            if (CommonValues.bGermanLanguage == true)
            {
                gameInfoText.text = "INFO: AN";
            }
            else
            {
                gameInfoText.text = "INFO: ON";
            }
            //without for() loop: because every info should shown once
        }
        else
        {
            CommonValues.bInfoDeactive = true;
            debounceButtonTime = 1f;
            if (CommonValues.bGermanLanguage == true)
            {
                gameInfoText.text = "INFO: AUS";
            }
            else
            {
                gameInfoText.text = "INFO: OFF";
            }
            for (int i = 0; i < CommonValues.bSceneInfoArray.Length; i++)
            {
                CommonValues.bSceneInfoArray[i] = true;
            }
        }
        if (PlayerPrefs.GetInt("MusicDeactive") == 0)
        {
            CommonValues.bMusicDeactive = false;
            if (CommonValues.bGermanLanguage == true)
            {
                gameMusicText.text = "MUSIK: AN";
            }
            else
            {
                gameMusicText.text = "MUSIC: ON";
            }
        }
        else
        {
            CommonValues.bMusicDeactive = true;
            if (CommonValues.bGermanLanguage == true)
            {
                gameMusicText.text = "MUSIK: AUS";
            }
            else
            {
                gameMusicText.text = "MUSIC: OFF";
            }
        }
    }

    public void ClickNewGameButton()
    {
        // if button is active
        if (CommonValues.bPopUpActive == false && debounceButtonTime <= 0f)
        {
            // reset player parameter
            int index_counter = 1;
            for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
            {
                CommonValues.PlayerParameterArray[i].index = index_counter;
                CommonValues.PlayerParameterArray[i].state = 0;
                CommonValues.PlayerParameterArray[i].score = 0;
                CommonValues.PlayerParameterArray[i].score_ranking = 0;
                CommonValues.PlayerParameterArray[i].death_ranking = 0;
                CommonValues.PlayerParameterArray[i].ranking_last_game = 0;
                CommonValues.PlayerParameterArray[i].mode = 0;
                CommonValues.PlayerParameterArray[i].team = 0;
                CommonValues.PlayerParameterArray[i].team_score = 0;
                CommonValues.PlayerParameterArray[i].team_ranking = 0;
                CommonValues.PlayerParameterArray[i].team_score_last_game = 0;
                CommonValues.PlayerParameterArray[i].team_ranking_last_game = 0;
                index_counter += 1;
            }

            // reset team player values 
            for (int i = 0; i < CommonValues.MaxNumberOfTeams; i++)
            {
                CommonValues.TeamParameterArray[i].member = 0;
                CommonValues.TeamParameterArray[i].active_member = 0;
            }

            // reset number of sessions
            CommonValues.iNumberOfRounds = 0;

            // load new scene
            SceneManager.LoadScene(CommonValues.sSceneArray[1]);
        }
    }

    public void ClickExitButton()
    {
        // if button is active
        if (CommonValues.bPopUpActive == false && debounceButtonTime <= 0f)
        {
            if (CommonValues.bMusicDeactive == false)
            {
                PlayerPrefs.SetInt("MusicDeactive", 0);
            }
            else
            {
                PlayerPrefs.SetInt("MusicDeactive", 1);
            }
            if (CommonValues.bInfoDeactive == false)
            {
                PlayerPrefs.SetInt("InfoDeactive", 0);
            }
            else
            {
                PlayerPrefs.SetInt("InfoDeactive", 1);
            }
            Application.Quit();
        }
    }

    IEnumerator delayedPopUpChange()
    {
        if(iPopUpCounter > 0)
        {
            if (CommonValues.bGermanLanguage == false)
            {
                oCanvasObjectArray[iPopUpCounter].gameObject.SetActive(false);
                oCanvasObjectArray[iPopUpCounter - 1].gameObject.SetActive(true);
            }
            else
            {
                oCanvasObjectGermanArray[iPopUpCounter].gameObject.SetActive(false);
                oCanvasObjectGermanArray[iPopUpCounter - 1].gameObject.SetActive(true);
            }
            audioManagerComp.playMaskDisapear();
        }
        else
        {
            if (CommonValues.bGermanLanguage == false)
            {
                oCanvasObjectArray[iPopUpCounter].gameObject.SetActive(false);
            }
            else
            {
                oCanvasObjectGermanArray[iPopUpCounter].gameObject.SetActive(false);
            }
            CommonValues.bSceneInfoArray[0] = true;
            PlayerPrefs.SetInt("PlayWithoutCircl", 0);

            // load saved language settings
            GetSavedSettings();

            //// show player assignment
            mask.GetComponent<Animator>().SetTrigger("ClearMask");
            //maskPopUpPlayerAssignment.SetActive(true);
            audioManagerComp.playMaskDisapear();
            //popUpPlayerAssignment.SetActive(true);
            CommonValues.bPopUpActive = false;
            SetNewGameButtonActive();
        }
        yield return new WaitForSeconds(2f);
        isCoroutineStarted = false;
        iPopUpCounter -= 1;
    }
}
