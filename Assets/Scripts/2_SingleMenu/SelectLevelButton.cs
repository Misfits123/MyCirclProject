using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SelectLevelButton : MonoBehaviour {
    // public variables
    // ----------------------------------------------------------------------------------------------------------------------------
    public GameObject[] oLevelArray;
    
    // privat variables
    // ----------------------------------------------------------------------------------------------------------------------------
    int DifficultyLevel = 0;
    Image DifficultyButtonImage;

    void Awake()
    {
        // set last difficulty level
        DifficultyLevel = CommonValues.iDifficultyLevel;

        // set selected game object
        DifficultyButtonImage = gameObject.GetComponent<Image>();

        // set last difficulty level image
        DifficultyButtonImage.sprite = oLevelArray[DifficultyLevel].GetComponent<Image>().sprite;
    }

    public void ClickButton()
    {
        // if button is active
        if (CommonValues.bSceneInfoArray[2] == true && CommonValues.bPopUpActive == false)
        {
            if (DifficultyLevel == oLevelArray.Length - 1)
            {
                DifficultyLevel = 0;
            }
            else
            {
                DifficultyLevel += 1;
            }

            DifficultyButtonImage.sprite = oLevelArray[DifficultyLevel].GetComponent<Image>().sprite;
            CommonValues.iDifficultyLevel = DifficultyLevel;
        }
    }
}
