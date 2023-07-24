using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    // public variables
    // ----------------------------------------------------------------------------------------------------------------------------
    public GameObject[] oPlayerArray;                               // reference to player prefabs
    public GameObject[] oPlayerWallArray;                           // reference to player wall prefabs
    public GameObject[] oPlayerHealthArray;                         // reference to player health prefabs
    public Material[] oPlayerMaterialArray;
    public Transform[] oPlayerSpawnPointArray;                      // reference to an array of the spawn points this player can spawn from values
    public Transform[] oPlayerWallSpawnPointArray;                  // reference to an array of the spawn points this player wall can spawn from values
    public Transform[] oPlayerHealthSpawnPointArray;                // reference to an array of the spawn points this player health can spawn from values
    public Image oDamageImage;                                      // reference to the UI's damage image value
    public GameObject[] oPlayerColorArray = new GameObject[CommonValues.MaxNumberOfPlayers];
    public GameObject[] oActivePlayerArray = new GameObject[CommonValues.MaxNumberOfPlayers];
    public GameObject[] oActivePlayerWallArray = new GameObject[CommonValues.MaxNumberOfPlayers];
    public PlayerMovement[] oActivePlayerMovementCompArray = new PlayerMovement[CommonValues.MaxNumberOfPlayers];
    public PlayerHealth[] oActivePlayerHealthCompArray = new PlayerHealth[CommonValues.MaxNumberOfPlayers];

    void Start()
    {
        // set player data
        for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
        {
            CommonValues.PlayerParameterArray[i].state = 0;
            if (CommonValues.PlayerParameterArray[i].team > 0)
            {
                CommonValues.TeamParameterArray[CommonValues.PlayerParameterArray[i].team - 1].active_member = 0;
            }
            CommonValues.PlayerParameterArray[i].color_text = oPlayerColorArray[i].GetComponent<Text>().text;
            CommonValues.PlayerParameterArray[i].color = oPlayerColorArray[i].GetComponent<SpriteRenderer>().color;
        }

        // set player setup
        addActivePlayer();
        addInactivePlayer();
    }

    public void addActivePlayer()
    {
        for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
        {
            if((CommonValues.iNumberOfRounds < CommonValues.iMaxNumberOfRounds && CommonValues.PlayerParameterArray[i].mode == 1) || CommonValues.PlayerParameterArray[i].mode == 3)
            {
                CommonValues.PlayerParameterArray[i].state = 1;
                oActivePlayerArray[i] = Instantiate(oPlayerArray[i], oPlayerSpawnPointArray[i].position, oPlayerSpawnPointArray[i].rotation);
                oActivePlayerWallArray[i] = Instantiate(oPlayerWallArray[i], oPlayerWallSpawnPointArray[i].position, oPlayerWallSpawnPointArray[i].rotation);
                Instantiate(oPlayerHealthArray[i], oPlayerHealthSpawnPointArray[i].position, oPlayerHealthSpawnPointArray[i].rotation);
                oActivePlayerMovementCompArray[i] = oActivePlayerArray[i].GetComponent<PlayerMovement>();
                oActivePlayerHealthCompArray[i] = oActivePlayerWallArray[i].GetComponent<PlayerHealth>();

                // set team color
                if (CommonValues.GameMode == 2)
                {
                    CommonValues.TeamParameterArray[CommonValues.PlayerParameterArray[i].team - 1].active_member += 1;
                    Material o_target_material = oPlayerMaterialArray[CommonValues.PlayerParameterArray[i].team - 1];
                    oActivePlayerArray[i].transform.gameObject.GetComponent<MeshRenderer>().material = o_target_material;
                    oActivePlayerWallArray[i].transform.gameObject.GetComponent<MeshRenderer>().material = o_target_material;
                }
            }
        }
    }

    public void addInactivePlayer()
    {
        for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
        {
            if ((CommonValues.iNumberOfRounds < CommonValues.iMaxNumberOfRounds && CommonValues.PlayerParameterArray[i].mode == 0) || (CommonValues.iNumberOfRounds >= CommonValues.iMaxNumberOfRounds && (CommonValues.PlayerParameterArray[i].mode == 0 || CommonValues.PlayerParameterArray[i].mode == 1)))
            {
                Instantiate(oPlayerWallArray[CommonValues.MaxNumberOfPlayers], oPlayerWallSpawnPointArray[i].position, oPlayerWallSpawnPointArray[i].rotation);
            }
        }
    }

    public GameObject[] getActivePlayerArray()
    {
        return oActivePlayerArray;
    }

    public PlayerHealth[] getActivePlayerHealthCompArray()
    {
        return oActivePlayerHealthCompArray;
    }
}