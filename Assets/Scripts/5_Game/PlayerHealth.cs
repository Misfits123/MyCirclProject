using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // privat variables
    // ----------------------------------------------------------------------------------------------------------------------------
    GameObject oPlayerManagerObj;                               // Reference to PlayerManager object.
    PlayerManager oPlayerManagerComp;                           // Reference to PlayerManager component.
    GameObject oPlayerObj;                                      // Reference to PlayerManager object.
    GameObject oHealthObj;                                    
    GameObject oAudioManagerObj;
    AudioManager oAudioManagerComp;                             // Reference to the AudioSource component.
    Vector3 oHealthVector = new Vector3(2.75f, 0.1f, 1f);
    int iPlayerIndex = 0;

    void Awake()
    {
        // Setting up the references.
        oPlayerManagerObj = GameObject.Find("PlayerManager");
        oPlayerManagerComp = oPlayerManagerObj.GetComponent<PlayerManager>();
        oAudioManagerObj = GameObject.Find("AudioManager");
        oAudioManagerComp = oAudioManagerObj.GetComponent<AudioManager>();

        // set player index
        string s_player_name = gameObject.name;
        char[] c_player_name_array = s_player_name.ToCharArray();
        for (int i = 0; i < c_player_name_array.Length; i++)
        {
            if (Char.IsDigit(c_player_name_array[i]))
            {
                iPlayerIndex = c_player_name_array[i] - '0';
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        oPlayerObj = GameObject.Find(oPlayerManagerComp.oPlayerArray[iPlayerIndex - 1].name + "(Clone)");
        if (collision.gameObject.name.Contains("Ball") && CommonValues.PlayerParameterArray[iPlayerIndex - 1].state != 0)
        {
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        if (CommonValues.getGameState() != CommonValues.State.GAMEOVER)
        {
            // select the right health slider
            oHealthObj = GameObject.Find(oPlayerManagerComp.oPlayerHealthArray[iPlayerIndex - 1].name + "(Clone)");

            // reduce the current health by the damage amount.
            float amount = 2.75f;
            oHealthVector.x += amount;
            oHealthObj.GetComponent<Transform>().transform.localScale = oHealthVector;

            // play the hurt sound effect.
            oAudioManagerComp.playSoundHurt();

            // if the player has lost all it's health and the death flag hasn't been set yet...
            if (oHealthVector.x >= 10f && CommonValues.PlayerParameterArray[iPlayerIndex - 1].state != 0)
            {
                // set player states
                CommonValues.PlayerParameterArray[iPlayerIndex - 1].state = 0;

                // set player score
                CommonValues.PlayerParameterArray[iPlayerIndex - 1].score += CommonValues.iNumberOfDeaths;

                // set player death ranking
                CommonValues.PlayerParameterArray[iPlayerIndex - 1].death_ranking = CommonValues.iNumberOfDeaths;

                // set ranking of last match
                CommonValues.PlayerParameterArray[iPlayerIndex - 1].ranking_last_game = CommonValues.NumberOfPlayers - CommonValues.iNumberOfDeaths;

                // increase number of deaths
                CommonValues.iNumberOfDeaths += 1;

                // decrease number of team member
                if (CommonValues.GameMode == 2)
                {
                    CommonValues.TeamParameterArray[CommonValues.PlayerParameterArray[iPlayerIndex - 1].team - 1].active_member -= 1;
                }
                // should die
                Death();
            }
        }
    }

    void Death()
    {
        // set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        oAudioManagerComp.playSoundDie();

        // delete Player
        Destroy(oPlayerObj);
    }
}
