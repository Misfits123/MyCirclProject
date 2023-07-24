using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    // public variables
    // ----------------------------------------------------------------------------------------------------------------------------
    public Transform[] oPlayerScoreSpawnPointArray;                      // reference to an array of the spawn points this player can spawn from values
    public GameObject[] oPlayerScore;                                    // the prefab to be spawned
    public GameObject[] oActiveScoreArray = new GameObject[CommonValues.MaxNumberOfPlayers];

    // privat variables
    // ----------------------------------------------------------------------------------------------------------------------------
    private GameObject oPlayerPivotObj;
    private GameObject oVictoryPanelObj;
    private GameObject oVictoryTextObj;
    private GameObject oVictoryPlayerTextObj;
    private Vector3 oScoreVector = new Vector3(1.0f, 1.0f, 1.0f);
    private Animator oVictoryAnimComp;
    private Text oVictoryTextComp;
    private Text oVictoryPlayerTextComp;

    void Awake()
    {
        // set references
        oVictoryPanelObj = GameObject.Find("VictoryPanel");
        oVictoryAnimComp = oVictoryPanelObj.GetComponent<Animator>();
        oVictoryTextObj = oVictoryPanelObj.transform.Find("VictoryText").gameObject;
        oVictoryTextComp = oVictoryTextObj.GetComponent<Text>();
        oVictoryPlayerTextObj = oVictoryPanelObj.transform.Find("PlayerText").gameObject;
        oVictoryPlayerTextComp = oVictoryPlayerTextObj.GetComponent<Text>();
    }

    void Start()
    {
        if (CommonValues.GameMode == 1)
        {
            // set player text and score
            for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
            {
                if (CommonValues.PlayerParameterArray[i].mode == 1)
                {
                    // set the right position according to the player position
                    oActiveScoreArray[i] = Instantiate(oPlayerScore[0], oPlayerScoreSpawnPointArray[i]);
                    oActiveScoreArray[i].transform.SetParent(oPlayerScoreSpawnPointArray[i], false);
                    oActiveScoreArray[i].name = "PlayerScore" + i.ToString();

                    // set current score
                    float f_score = (float)CommonValues.PlayerParameterArray[i].score;
                    GameObject o_text = oActiveScoreArray[i].transform.Find("PlayerText").gameObject;
                    GameObject o_score = oActiveScoreArray[i].transform.Find("PlayerScore").gameObject;
                    o_text.GetComponent<Text>().color = CommonValues.PlayerParameterArray[i].color;
                    o_score.GetComponent<Text>().color = CommonValues.PlayerParameterArray[i].color;
                    if(CommonValues.bGermanLanguage == false)
                    {
                        o_score.GetComponent<Text>().text = "SCORE: " + f_score.ToString();
                        o_text.GetComponent<Text>().text = "RANK: " + CommonValues.PlayerParameterArray[i].score_ranking.ToString();
                    }
                    else
                    {
                        o_score.GetComponent<Text>().text = "PUNKTE: " + f_score.ToString();
                        o_text.GetComponent<Text>().text = "PLATZ: " + CommonValues.PlayerParameterArray[i].score_ranking.ToString();
                        o_score.GetComponent<Text>().fontSize = 27;
                        o_text.GetComponent<Text>().fontSize = 27;
                    }

                    // set winner and loser animation
                    if (CommonValues.PlayerParameterArray[i].score_ranking == 1)
                    {
                        oActiveScoreArray[i].GetComponent<Animator>().SetTrigger("TriggerPulsing");
                    }
                    else if (CommonValues.PlayerParameterArray[i].score_ranking == CommonValues.NumberOfPlayers)
                    {
                        oActiveScoreArray[i].GetComponent<Animator>().SetTrigger("TriggerShaking");
                    }

                }
            }

            // set winner of last game in victory panel
            if (CommonValues.getGameState() == CommonValues.State.GAMEOVER)
            {
                for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
                {
                    // last man standing
                    if (CommonValues.PlayerParameterArray[i].state == 1)
                    {
                        oVictoryPlayerTextComp.text = CommonValues.PlayerParameterArray[i].color_text;
                        oVictoryPlayerTextComp.color = CommonValues.PlayerParameterArray[i].color;
                    }
                }
            }
            else if (CommonValues.getGameState() == CommonValues.State.TIMEUP || CommonValues.getGameState() == CommonValues.State.DRAWN)
            {
                oVictoryTextComp.text = "";
                if (CommonValues.bGermanLanguage == false)
                {
                    oVictoryPlayerTextComp.text = "DRAWN";
                }
                else
                {
                    oVictoryPlayerTextComp.text = "UNENTSCHIEDEN";
                    oVictoryPlayerTextComp.fontSize = 80;
                }
            }
            else
            {
                // useful for other game states
            }
        }
        else if (CommonValues.GameMode == 2)
        {
            // set player text and team score
            for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
            {
                if (CommonValues.PlayerParameterArray[i].mode == 1)
                {
                    // get team
                    int i_team = CommonValues.PlayerParameterArray[i].team;

                    // set the right position according to the player position
                    oActiveScoreArray[i] = Instantiate(oPlayerScore[0], oPlayerScoreSpawnPointArray[i]);
                    oActiveScoreArray[i].transform.SetParent(oPlayerScoreSpawnPointArray[i], false);
                    GameObject o_text = oActiveScoreArray[i].transform.Find("PlayerText").gameObject;
                    GameObject o_score = oActiveScoreArray[i].transform.Find("PlayerScore").gameObject;
                    o_text.GetComponent<Text>().color = CommonValues.PlayerParameterArray[i_team - 1].color;
                    o_score.GetComponent<Text>().color = CommonValues.PlayerParameterArray[i_team - 1].color;
                    oActiveScoreArray[i].name = "PlayerScore" + i.ToString();
                    if(CommonValues.bGermanLanguage == false)
                    {
                        o_score.GetComponent<Text>().text = "SCORE: " + CommonValues.PlayerParameterArray[i].team_score.ToString();
                        o_text.GetComponent<Text>().text = "RANK: " + CommonValues.PlayerParameterArray[i].team_ranking.ToString();
                    }
                    else
                    {
                        o_score.GetComponent<Text>().text = "PUNKTE: " + CommonValues.PlayerParameterArray[i].team_score.ToString();
                        o_text.GetComponent<Text>().text = "PLATZ: " + CommonValues.PlayerParameterArray[i].team_ranking.ToString();
                        o_score.GetComponent<Text>().fontSize = 27;
                        o_text.GetComponent<Text>().fontSize = 27;
                    }

                    // set winner and loser animation
                    if (CommonValues.PlayerParameterArray[i].team_ranking == 1)
                    {
                        oActiveScoreArray[i].GetComponent<Animator>().SetTrigger("TriggerPulsing");
                    }
                    else if(CommonValues.PlayerParameterArray[i].team_ranking == CommonValues.NumberOfTeams)
                    {
                        oActiveScoreArray[i].GetComponent<Animator>().SetTrigger("TriggerShaking");
                    }
                }
            }

            // set winner of last game in victory panel
            if(CommonValues.getGameState() == CommonValues.State.GAMEOVER)
            {
                for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
                {
                    // get team
                    int i_team = CommonValues.PlayerParameterArray[i].team - 1;

                    // last man standing
                    if (CommonValues.PlayerParameterArray[i].state == 1)
                    {
                        oVictoryPlayerTextComp.text = CommonValues.PlayerParameterArray[i_team].color_text;
                        oVictoryPlayerTextComp.color = CommonValues.PlayerParameterArray[i_team].color;
                    }
                }
            }
            else if (CommonValues.getGameState() == CommonValues.State.TIMEUP || CommonValues.getGameState() == CommonValues.State.DRAWN)
            {
                oVictoryTextComp.text = "";
                if (CommonValues.bGermanLanguage == false)
                {
                    oVictoryPlayerTextComp.text = "DRAWN";
                }
                else
                {
                    oVictoryPlayerTextComp.text = "UNENTSCHIEDEN";
                    oVictoryPlayerTextComp.fontSize = 80;
                }
            }
            else
            {
                // useful for other game states
            }

        }
        else
        {
            //useful for other game modes
        }
    }
}
