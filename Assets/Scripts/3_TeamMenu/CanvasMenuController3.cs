using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasMenuController3 : MonoBehaviour
{
    // public static variables
    // ----------------------------------------------------------------------------------------------------------------------------
    // public variables
    // ----------------------------------------------------------------------------------------------------------------------------
    public GameObject[] CanvasObjectArray;
    public Image mask;

    // private variables
    // ----------------------------------------------------------------------------------------------------------------------------
    GameObject oAudioManagerObj;
    AudioManager oAudioManagerComp;

    // start is called before the first frame update
    void Awake()
    {
        // set references
        oAudioManagerObj = GameObject.Find("AudioManager");
        oAudioManagerComp = oAudioManagerObj.GetComponent<AudioManager>();

        // show menu infos just once
        if (CommonValues.bSceneInfoArray[3] == true)
        {
            for (int j = 0; j < CanvasObjectArray.Length; j++)
            {
                CanvasObjectArray[j].gameObject.SetActive(false);
            }
            mask.GetComponent<Animator>().SetTrigger("ClearMask");
        }
        else
        {
            oAudioManagerComp.playGameInfoApear();
            if (CommonValues.bGermanLanguage == false)
            {
                CanvasObjectArray[0].gameObject.SetActive(true);
            }
            else
            {
                CanvasObjectArray[1].gameObject.SetActive(true);
            }
        }
    }

    // update is called once per frame
    void Update()
    {
        if(CommonValues.bSceneInfoArray[3] == false && CommonValues.bPopUpActive == false)
        {
            // close info text
            for(int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
            {
                if (CirCl.GetButtonDown(i, 0, false))
                {
                    for(int j = 0; j < CanvasObjectArray.Length; j++)
                    {
                        CanvasObjectArray[j].gameObject.SetActive(false);
                    }
                    mask.GetComponent<Animator>().SetTrigger("ClearMask");
                    oAudioManagerComp.playMaskDisapear();

                    // set scene info inactive
                    CommonValues.bSceneInfoArray[3] = true;
                }
            }
        }
    }
}
