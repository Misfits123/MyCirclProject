using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // public variables
    // ----------------------------------------------------------------------------------------------------------------------------
    public static float time;
    public float fRestartDelay = 5f;        // time to wait before restarting the level

    // privat variables
    // ----------------------------------------------------------------------------------------------------------------------------
    [SerializeField]
    private float timeLimit = 200.0f;
    private float timeExit = 3.0f;
    private Image timeExitBar;
    private PlayerManager oPlayerManagerComp;               // reference to PlayerManager component
    private PlayerHealth[] oActivePlayerHealthCompArray;    // reference to Player component
    private AudioManager oAudioManagerComp;                 // reference to the AudioSource component
    private Animator oAnim;                                 // reference to the animator component
    private bool isCoroutineStarted = false;

    void Awake()
    {
        // reset number of balls
        CommonValues.NumberOfBalls = 0;

        // reset number of items
        CommonValues.NumberOfItems = 0;

        // set up the references
        oPlayerManagerComp = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        oActivePlayerHealthCompArray = oPlayerManagerComp.getActivePlayerHealthCompArray();
        oAudioManagerComp = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        timeExitBar = GameObject.Find("MainMenuBar").GetComponent<Image>();
        oAnim = GetComponent<Animator>();

        // set game state init
        CommonValues.setGameState(CommonValues.State.INIT);

        // stop game menu music
        oAudioManagerComp.stopGameMenuMusic();
        oAudioManagerComp.stopGameMenuMusic2();

        // set difficulty level
        if (CommonValues.iDifficultyLevel == 0)
        {
            // set easy
            CommonValues.MaxNumberOfItems = 0;
            CommonValues.MaxNumberOfBalls = 1;
        }
        else if (CommonValues.iDifficultyLevel == 1)
        {
            // set medium
            CommonValues.MaxNumberOfItems = 2;
            CommonValues.MaxNumberOfBalls = 1;
        }
        else
        {
            //set hard
            CommonValues.MaxNumberOfItems = 3;
            CommonValues.MaxNumberOfBalls = 3;
        }
    }
    void Start()
    {
        time = timeLimit;
    }
    void Update()
    {
        // exit game by pressing two buttons
        if ((CirCl.GetButton(CommonValues.iEventSystemPlayer, 0) && CirCl.GetButton(CommonValues.iEventSystemPlayer, 1)))
        {
            timeExit -= Time.deltaTime;
            timeExitBar.fillAmount += Time.deltaTime / 3.0f;

            if (timeExit < 0f)
            {
                oAudioManagerComp.stopGameMusic1();
                // load main menu
                SceneManager.LoadScene(CommonValues.sSceneArray[0]);
            }
        }
        else
        {
            timeExit = 3.0f;
            timeExitBar.fillAmount = 0f;
        }
    }
    void FixedUpdate()
    {
        if (CommonValues.getGameState() == CommonValues.State.START)
        {
            oAudioManagerComp.playGameMusic1();
            CommonValues.setGameState(CommonValues.State.PLAY);
        }
        else if (CommonValues.getGameState() == CommonValues.State.PLAY)
        {
            time -= Time.deltaTime;

            if(time < 0 && isCoroutineStarted == false)
            {
                // set time zero
                time = 0;

                // set timeup state
                CommonValues.setGameState(CommonValues.State.TIMEUP);

                // set the same score to all active player
                for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
                {
                    // if player is alive
                    if (CommonValues.PlayerParameterArray[i].mode == 1 || CommonValues.PlayerParameterArray[i].mode == 3)
                    {
                        // set player score
                        CommonValues.PlayerParameterArray[i].score = CommonValues.PlayerParameterArray[i].score - CommonValues.PlayerParameterArray[i].death_ranking + 1;

                        // set player death ranking
                        CommonValues.PlayerParameterArray[i].death_ranking = 1;

                        // set ranking of last match
                        CommonValues.PlayerParameterArray[i].ranking_last_game = 1;
                    }
                }

                // reset number of deaths
                CommonValues.iNumberOfDeaths = 0;

                // load score menu
                isCoroutineStarted = true;
                StartCoroutine(delayedSceneChange());
            }

            // check if the game is over
            CommonMethods.checkGameIsOver();
        }
        else if(CommonValues.getGameState() == CommonValues.State.GAMEOVER || CommonValues.getGameState() == CommonValues.State.DRAWN && isCoroutineStarted == false)
        {
            isCoroutineStarted = true;
            StartCoroutine(delayedSceneChange());
        }
        else
        {
            // do nothing
        }
    }
    IEnumerator delayedSceneChange()
    {
        yield return new WaitForSeconds(4.0f);
        oAudioManagerComp.stopGameMusic1();
        CommonMethods.setPlayerRanking();
        CommonMethods.setTeamRanking();
        CommonValues.iNumberOfRounds += 1;
        if (CommonValues.iNumberOfRounds >= CommonValues.iMaxNumberOfRounds)
        {
            int[,] rankings = CommonMethods.countRankings();
            int first_place_count = 0;
            int second_place_count = 0;
            int third_place_count = 0;
            if (CommonValues.GameMode == 1)
            {
                first_place_count = rankings[0, CommonValues.MaxNumberOfPlayers];
                second_place_count = rankings[1, CommonValues.MaxNumberOfPlayers];
                third_place_count = rankings[2, CommonValues.MaxNumberOfPlayers];
            }
            else
            {
                first_place_count = rankings[0, CommonValues.MaxNumberOfPlayers + 1];
                second_place_count = rankings[1, CommonValues.MaxNumberOfPlayers + 1];
                third_place_count = rankings[2, CommonValues.MaxNumberOfPlayers + 1];
            }

            if (third_place_count > 1)
            {
                for (int i = 0; i < rankings[2, CommonValues.MaxNumberOfPlayers]; i++)
                {
                    CommonValues.PlayerParameterArray[rankings[2, i] - 1].mode = 3;
                }
                CommonValues.sCanvasSuddenDeathText = "K.O. MATCH";
                CommonValues.iCanvasSuddenDeathTextSize = 200;
                SceneManager.LoadScene(CommonValues.sSceneArray[5]);
            }
            else if (second_place_count > 1)
            {
                for (int i = 0; i < rankings[1, CommonValues.MaxNumberOfPlayers]; i++)
                {
                    CommonValues.PlayerParameterArray[rankings[1, i] - 1].mode = 3;
                }
                CommonValues.sCanvasSuddenDeathText = "K.O. MATCH";
                CommonValues.iCanvasSuddenDeathTextSize = 200;
                SceneManager.LoadScene(CommonValues.sSceneArray[5]);
            }
            else if (first_place_count > 1)
            {
                for (int i = 0; i < rankings[0, CommonValues.MaxNumberOfPlayers]; i++)
                {
                    CommonValues.PlayerParameterArray[rankings[0, i] - 1].mode = 3;
                }
                CommonValues.sCanvasSuddenDeathText = "K.O. MATCH";
                CommonValues.iCanvasSuddenDeathTextSize = 200;
                SceneManager.LoadScene(CommonValues.sSceneArray[5]);
            }
            else
            {
                CommonValues.sCanvasSuddenDeathText = "OK";
                CommonValues.iCanvasSuddenDeathTextSize = 200;
                SceneManager.LoadScene(CommonValues.sSceneArray[7]);
            }
        }
        else
        {
            CommonValues.sCanvasSuddenDeathText = "OK";
            CommonValues.iCanvasSuddenDeathTextSize = 200;
            SceneManager.LoadScene(CommonValues.sSceneArray[6]);
        }
    }
}
