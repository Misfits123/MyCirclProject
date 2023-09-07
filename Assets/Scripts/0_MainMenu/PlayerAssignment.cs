using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerAssignment : MonoBehaviour
{
    public Text titelMessage;
    public GameObject joystickMessage;
    public Text notConnectedMessage;
    public GameObject[] joystickInfoArray;
    public Color[] playerColorArray;

    private GameObject audioManagerObj;
    private AudioManager audioManagerComp;
    private StandaloneInputModule eventSystemPlayer;
    private int joystickCounter;
    private string[,] joystickNewArray;
    private bool[] joystickSetArray;
    public bool setEventSystemPlayer;

    private void Start()
    {
        // set references
        audioManagerObj = GameObject.Find("AudioManager");
        audioManagerComp = audioManagerObj.GetComponent<AudioManager>();
        eventSystemPlayer = GameObject.Find("EventSystem").GetComponent<StandaloneInputModule>();

        joystickCounter = 1;
    }

    //private void OnEnable()
    //{
    //    CommonValues.bPopUpActive = true;

    //    if (CommonValues.bGermanLanguage == true)
    //    {
    //        titelMessage.text = "SPIELER ZUORDNUNG";
    //    }
    //    else
    //    {
    //        titelMessage.text = "PLAYER ASSIGNMENT";
    //    }

    //    joystickCounter = 1;
    //    SetJoystickInfo();

    //    joystickNewArray = new string[,] {
    //    { "P9_Horizontal", "P9_Vertical", "P9_Button1", "P9_Button0" },
    //    { "P9_Horizontal", "P9_Vertical", "P9_Button1", "P9_Button0" },
    //    { "P9_Horizontal", "P9_Vertical", "P9_Button1", "P9_Button0" },
    //    { "P9_Horizontal", "P9_Vertical", "P9_Button1", "P9_Button0" },
    //    { "P9_Horizontal", "P9_Vertical", "P9_Button1", "P9_Button0" },
    //    { "P9_Horizontal", "P9_Vertical", "P9_Button1", "P9_Button0" },
    //    { "P9_Horizontal", "P9_Vertical", "P9_Button1", "P9_Button0" },
    //    { "P9_Horizontal", "P9_Vertical", "P9_Button1", "P9_Button0" }
    //    };

    //    joystickSetArray = new bool[] { false, false, false, false, false, false, false, false };
    //}

    // Update is called once per frame
    private void Update()
    {

    }

    //public void ClickNoJoystick()
    //{
    //    PlayerPrefs.SetInt("PlayerPosition" + joystickCounter.ToString(), 9);
    //    audioManagerComp.playMaskDisapear();
    //    joystickCounter += 1;
    //    SetJoystickInfo();

    //    if (joystickCounter > 8)
    //    {
    //        gameObject.SetActive(false);
    //        audioManagerComp.playMaskDisapear();
    //        joystickMessage.GetComponent<Animator>().SetBool("Play", false);
    //        CommonValues.sControllerInputArray = joystickNewArray;
    //        PlayerPrefs.SetInt("PlayerAssignment", 1);
    //        SetCirclDetection();
    //        SetButtonActive();
    //        SetEventSystemPlayer();
    //        CommonValues.bPopUpActive = false;
    //    }
    //}

    private void SetJoystickInfo()
    {
        if (CommonValues.bGermanLanguage == true)
        {
            joystickMessage.GetComponent<Text>().text = "SPIELER AUF POSITION " + joystickCounter.ToString() + " DRÜCKE \"C\"";
            notConnectedMessage.text = "KEIN SPIELER AUF POSITION " + joystickCounter.ToString() + " - DRÜCKE CIRCL LOGO";
        }
        else
        {
            joystickMessage.GetComponent<Text>().text = "PLAYER AT POSITION " + joystickCounter.ToString() + " PRESS \"C\"";
            notConnectedMessage.text = "NO PLAYER AT POSITION " + joystickCounter.ToString() + " - PRESS CIRCL LOGO";
        }

        if(joystickCounter <= 8)
        {
            joystickMessage.GetComponent<Text>().color = playerColorArray[joystickCounter - 1];
            for(int i = 0; i < 8; i++)
            {
                if(i == joystickCounter - 1)
                {
                    joystickInfoArray[i].gameObject.GetComponent<Animator>().SetBool("Play", true);
                }
                else
                {
                    joystickInfoArray[i].gameObject.GetComponent<Animator>().SetBool("Play", false);
                }
            }
            joystickMessage.GetComponent<Animator>().SetBool("Play", true);
        }
    }

    //private void SetCirclDetection()
    //{
    //    CommonValues.bCirclDetected = false;
    //    string[] devices = Input.GetJoystickNames();
    //    for (int i = 0; i < devices.Length; i++)
    //    {
    //        if (devices[i].Contains("CirCl"))
    //        {
    //            CommonValues.bCirclDetected = true;
    //        }
    //    }
    //}

    private void SetButtonActive()
    {
        string buttonName = "";
        for(int i = 0; i < CommonValues.sSceneArray.Length; i++)
        {
            if (SceneManager.GetActiveScene().name == CommonValues.sSceneArray[i])
            {
                if(i == 0)
                {
                    buttonName = "NewGameButton";
                }
                else if(i == 1)
                {
                    buttonName = "SingleButton";
                }
                //else if (i == 2)
                //{
                //    buttonName = "OkButton";
                //}
                //else if (i == 3)
                //{
                //    buttonName = "OkButton";
                //}
                else if (i == 5)
                {
                    buttonName = "OkButton";
                }
                else if (i == 10)
                {
                    buttonName = "ContinueButton";
                }
                else
                {
                    buttonName = "";
                }
            }
        }

        // set button active
        if(buttonName != "")
        {
            GameObject.Find(buttonName).gameObject.GetComponent<Button>().interactable = true;
            GameObject.Find(buttonName).gameObject.GetComponent<Button>().Select();
            GameObject.Find(buttonName).gameObject.GetComponent<Button>().gameObject.SetActive(true);
        }
    }
}
