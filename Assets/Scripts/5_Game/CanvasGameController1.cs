using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGameController1 : MonoBehaviour {
    // public static variables
    // ----------------------------------------------------------------------------------------------------------------------------
    // public variables
    // ----------------------------------------------------------------------------------------------------------------------------
    public GameObject[] canvasObjectArray;
    public Image mask;
    public Text timeText;
    public Text time2Text;
    private AudioManager audioManager;
    private CountdownController1 countdownController;

    void Awake()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        countdownController = GameObject.Find("Countdown").GetComponent<CountdownController1>();

        // show menu infos just once
        if (CommonValues.bSceneInfoArray[6] == true)
        {
            for (int j = 0; j < canvasObjectArray.Length; j++)
            {
                canvasObjectArray[j].gameObject.SetActive(false);
            }
            mask.GetComponent<Animator>().SetTrigger("ClearMask");
            countdownController.bCountdownRunning = true;
        }
        else
        {
            audioManager.playGameInfoApear();
            if (CommonValues.bGermanLanguage == false)
            {
                canvasObjectArray[0].gameObject.SetActive(true);
            }
            else
            {
                canvasObjectArray[1].gameObject.SetActive(true);
            }
            countdownController.bCountdownRunning = false;
        }
    }

    void Update () 
    {
        // draw time
        this.timeText.text = GameManager.time.ToString("F0");
        this.time2Text.text = GameManager.time.ToString("F0");

        if (CommonValues.bSceneInfoArray[6] == false && CommonValues.bPopUpActive == false)
        {
            // close info text
            for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
            {
                if (CirCl.GetButtonDown(i, 0, false))
                {
                    for (int j = 0; j < canvasObjectArray.Length; j++)
                    {
                        canvasObjectArray[j].gameObject.SetActive(false);
                    }
                    mask.GetComponent<Animator>().SetTrigger("ClearMask");
                    audioManager.playMaskDisapear();

                    // starting countdown
                    countdownController.bCountdownRunning = true;

                    // set scene info inactive
                    CommonValues.bSceneInfoArray[6] = true;
                }
            }
        }
    }
}
