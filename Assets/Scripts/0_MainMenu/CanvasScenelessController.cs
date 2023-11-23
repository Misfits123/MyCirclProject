using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
//using UnityEngine.InputSystem;

public class CanvasScenelessController : MonoBehaviour {
    public GameObject canvasObjectPlayerAssignment;
    public GameObject canvasObjectPlayer;
    //public Text debugText;

    private AudioManager audioManager;
    private StandaloneInputModule eventSystemPlayer;
    private int sceneIndex;
    private bool popUpWarning = true;
    //private int debugCounter = 0;

    private void Awake()
    {
        if (CommonValues.bCreateCanvasScenelessOnce == false)
        {
            CommonValues.bCreateCanvasScenelessOnce = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(transform.gameObject);
        }
    }

    private void Start()
    {
        // set references
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        //CommonValues.bPopUpActive = false;
        //SetCirclDetection();
        popUpWarning = false;
    }

    private void Update()
    {
        if (CommonValues.bPlayWithoutCircl == false)
        {
            ////SetCirclDetection();
            //if (CommonValues.bCirclDetected == false)
            //{
            //    if (CommonValues.bPopUpActive == false)
            //    {
            //        popUpWarning = true;
            //        CommonValues.bPopUpActive = true;
            //        audioManager.playGameInfoApear();
            //        canvasObjectPlayer.SetActive(true);
            //        if (CommonValues.bGermanLanguage == true)
            //        {
            //            canvasObjectPlayer.transform.Find("Message").GetComponent<Text>().text = "BITTE PRÜFE DIE CIRCL VERBINDUNG";
            //        }
            //        else
            //        {
            //            canvasObjectPlayer.transform.Find("Message").GetComponent<Text>().text = "PLEASE CHECK THE CIRCL CONNECTION";
            //        }
            //    }
            //}
            //else
            //{
            //    if (popUpWarning == true)
            //    {
            //        popUpWarning = false;
            //        CommonValues.bPopUpActive = false;
            //        canvasObjectPlayer.SetActive(false);
            //    }
            //}

            //if (sceneIndex != SceneManager.GetActiveScene().buildIndex)
            //{
            //    sceneIndex = SceneManager.GetActiveScene().buildIndex;
            //    SetEventSystemPlayer();
            //}
        }
    }

    //private void SetEventSystemPlayer()
    //{
    //    eventSystemPlayer = GameObject.Find("EventSystem").gameObject.GetComponent<StandaloneInputModule>();
    //    eventSystemPlayer.horizontalAxis = CirCl.controllerArray[CommonValues.iEventSystemPlayer - 1, 0];
    //    eventSystemPlayer.verticalAxis = CirCl.controllerArray[CommonValues.iEventSystemPlayer - 1, 1];
    //    eventSystemPlayer.cancelButton = CirCl.controllerArray[CommonValues.iEventSystemPlayer - 1, 2];
    //    eventSystemPlayer.submitButton = CirCl.controllerArray[CommonValues.iEventSystemPlayer - 1, 3];
    //}
}
