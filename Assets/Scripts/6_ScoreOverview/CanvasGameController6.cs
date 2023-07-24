using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasGameController6 : MonoBehaviour {
    // public static variables
    // ----------------------------------------------------------------------------------------------------------------------------
    // public variables
    // ----------------------------------------------------------------------------------------------------------------------------
    // private variables
    // ----------------------------------------------------------------------------------------------------------------------------
    private AudioManager audioManagerComp;
    private Text RoundsText;
    private Text RoundsText2;
    private Text RoundsText3;
    private Text RoundsText4;
    private GameObject victoryPanel;

    // use this for initialization
    void Awake()
    {
        // set references
        audioManagerComp = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        RoundsText = GameObject.Find("RoundsText").GetComponent<Text>();
        RoundsText2 = GameObject.Find("RoundsText2").GetComponent<Text>();
        RoundsText3 = GameObject.Find("RoundsText3").GetComponent<Text>();
        RoundsText4 = GameObject.Find("RoundsText4").GetComponent<Text>();
        victoryPanel = GameObject.Find("VictoryPanel");
    }

    void Start()
    {
        RoundsText.gameObject.SetActive(false);
        RoundsText2.gameObject.SetActive(false);
        RoundsText3.gameObject.SetActive(false);
        RoundsText4.gameObject.SetActive(false);
        victoryPanel.SetActive(false);

        audioManagerComp.stopGameMusic1();
        audioManagerComp.stopGameMusic2();
        audioManagerComp.stopGameMenuMusic();

        if (CommonValues.sLastScene != CommonValues.sSceneArray[0])
        {
            victoryPanel.SetActive(true);
        }
        else
        {
            // set menu buttons active
            CommonValues.bSceneInfoArray[6] = true;
        }
        CommonValues.sLastScene = SceneManager.GetActiveScene().name;
    }

    void Update () 
    {
        if(CommonValues.bSceneInfoArray[6] == true && CommonValues.bPopUpActive == false)
        {
            // draw rounds
            RoundsText.gameObject.SetActive(true);
            RoundsText2.gameObject.SetActive(true);
            RoundsText3.gameObject.SetActive(true);
            RoundsText4.gameObject.SetActive(true);
            RoundsText.text = CommonValues.iNumberOfRounds.ToString() + "/" + CommonValues.iMaxNumberOfRounds.ToString();
            RoundsText3.text = CommonValues.iNumberOfRounds.ToString() + "/" + CommonValues.iMaxNumberOfRounds.ToString();
            if(CommonValues.bGermanLanguage == false)
            {
                RoundsText2.text = "ROUND";
                RoundsText4.text = "ROUND";
            }
            else
            {
                RoundsText2.text = "RUNDE";
                RoundsText4.text = "RUNDE";
            }
        }     
	}
}
