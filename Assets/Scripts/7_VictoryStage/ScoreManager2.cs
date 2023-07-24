using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ScoreManager2 : MonoBehaviour
{
    // public variables
    // ----------------------------------------------------------------------------------------------------------------------------
    public Transform[] oPlayerScoreSpawnPointArray;                      // reference to an array of the spawn points this player can spawn from values
    public GameObject[] oPlayerScore;                                    // the prefab to be spawned
    public GameObject[] oActiveScoreArray = new GameObject[CommonValues.MaxNumberOfPlayers];

    void Start()
    {
        for (int i = 0; i < oActiveScoreArray.Length; i++)
        {
            if(oActiveScoreArray[i] != null)
            {
                oActiveScoreArray[i].gameObject.SetActive(false);
            }
        }
    }
}
