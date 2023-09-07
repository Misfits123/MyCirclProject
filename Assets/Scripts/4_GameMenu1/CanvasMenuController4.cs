using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasMenuController4 : MonoBehaviour
{
    // public static variables
    // ----------------------------------------------------------------------------------------------------------------------------
    // public variables
    // ----------------------------------------------------------------------------------------------------------------------------
    public GameObject[] canvasObjectArray;
    public Image mask;

    // private variables
    // ----------------------------------------------------------------------------------------------------------------------------
    private GameObject audioManagerObj;
    private AudioManager audioManagerComp;
    private Color colorUnselected = new Color(1.0F, 1.0F, 1.0F, 1.0F);
    private Color colorSelected = new Color(0.4F, 0.4F, 0.4F, 0.8F);

    // start is called before the first frame update
    void Awake()
    {
        // set references
        audioManagerObj = GameObject.Find("AudioManager");
        audioManagerComp = audioManagerObj.GetComponent<AudioManager>();

        // show menu infos just once
        if (CommonValues.bSceneInfoArray[4] == true)
        {
            for (int j = 0; j < canvasObjectArray.Length; j++)
            {
                canvasObjectArray[j].gameObject.SetActive(false);
            }
            mask.GetComponent<Animator>().SetTrigger("ClearMask");
        }
        else
        {
            audioManagerComp.playGameInfoApear();
            if (CommonValues.bGermanLanguage == false)
            {
                canvasObjectArray[0].gameObject.SetActive(true);
            }
            else
            {
                canvasObjectArray[1].gameObject.SetActive(true);
            }
        }
    }

    void Update()
    {
        if (CommonValues.bSceneInfoArray[4] == false && CommonValues.bPopUpActive == false)
        {
            // close info text
            for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
            {
                if (CirCl.GetButtonDown(i, 0))
                {
                    for (int j = 0; j < canvasObjectArray.Length; j++)
                    {
                        canvasObjectArray[j].gameObject.SetActive(false);
                    }
                    mask.GetComponent<Animator>().SetTrigger("ClearMask");
                    audioManagerComp.playMaskDisapear();

                    // set scene info inactive
                    CommonValues.bSceneInfoArray[4] = true;
                }
            }
        }
    }
}
