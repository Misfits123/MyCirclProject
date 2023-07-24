using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

public class CommonValues : MonoBehaviour
{
    private static int iGameMode = 0;
    public static int iEventSystemPlayer = 1;
    public static int iNumberOfJoysticks = 1;
    public static bool bPopUpActive = false;
    public static bool bCreateCanvasScenelessOnce = false;
    public static bool bCreateAudioManagerOnce = false;
    public static string sCanvasSuddenDeathText = "OK";
    public static int iCanvasSuddenDeathTextSize = 200;
    public static bool bGermanLanguage = false;
    public static bool bMusicDeactive = false;
    public static bool bInfoDeactive = false;
    public static bool bPlayWithoutCircl = false;

    // if the maximum number of session is reached the score will be reset
    public static int iNumberOfRounds = 0;
    public static int iMaxNumberOfRounds = 6;
    public static int iNumberOfDeaths = 0;

    // XXX_SpeciallyForCircl_XXX: playing with up to 8 player
    private static int iMaxNumberOfPlayers = 8;
    private static int iNumberOfPlayers = 0;
    private static int iNumberOfActivePlayers = 0;
    private static int iMaxNumberOfBalls = 1;
    private static int iNumberOfBalls = 0;
    private static int iMaxNumberOfItems = 1;
    private static int iNumberOfItems = 0;
    private static int iPlayerLevel = 0;
    private static int iGameLevel = 0;
    public static int iMaxNumberOfGameLevel = 4;
    public static int iDifficultyLevel = 0;
    public static int iMaxNumberOfGameMods = 9;

    // player parameter = Index, State, Score, Score_Ranking, Death_Ranking, Ranking_Last_Game, Mode, Team, Team_Score, Team_Ranking, Team_Score_Last_Game, Team_Ranking_Last_Game
    public struct sPlayer
    {
        public int index;
        public int state;
        public int score;
        public int score_ranking;
        public int death_ranking;
        public int ranking_last_game;
        public int mode;
        public int team;
        public int team_score;
        public int team_ranking;
        public int team_score_last_game;
        public int team_ranking_last_game;
        public string color_text;
        public Color color;
        public Transform position;
    }
    public static sPlayer[] PlayerParameterArray = new sPlayer[iMaxNumberOfPlayers];
    private static float fCreateItemTimer = 10f;
    private static int iMaxNumberOfTeams = 4;
    private static int iNumberOfTeams = 0;
    private static int iNumberOfActiveTeams = 0;

    // team parameter = Member, Active_Member
    public struct sTeam
    {
        public int member;
        public int active_member;
    }
    public static sTeam[] TeamParameterArray = new sTeam[iMaxNumberOfTeams];

    // XXX_SpeciallyForCircl_XXX: all player are playing from different perspectives
    public static float[] fPlayerViewpointArray = new float[] { 0.0f, 180.0f, 90.0f, -90.0f, 45.0f, -135.0f, 135.0f, -45.0f };

    public static float[] fPlayerViewpointArray2 = new float[] { 0.0f, 180.0f, 90.0f, 270.0f, 45.0f, 225.0f, 135.0f, 315.0f };

    // all scenes
    public static string[] sSceneArray = new string[] { "0_MainMenu", "1_ModeMenu", "2_SingleMenu", "3_TeamMenu", "4_GameMenu1", "5_Game", "6_ScoreOverview", "7_VictoryStage" };

    // all scene infos
    public static bool[] bSceneInfoArray = new bool[sSceneArray.Length];

    // last scene
    public static string sLastScene;

    // choosen game
    public static int iGameIndex = 0;

    public enum State
    {
        INIT,
        START,
        SET,
        PLAY,
        TIMEUP,
        DRAWN,
        GAMEOVER,
    }
    private static State GameState;

    public static int GameMode
    {
        get
        {
            return iGameMode;
        }
        set
        {
            iGameMode = value;
        }
    }

    public static int NumberOfPlayers
    {
        get
        {
            return iNumberOfPlayers;
        }
        set
        {
            iNumberOfPlayers = value;
        }
    }

    public static int NumberOfActivePlayers
    {
        get
        {
            return iNumberOfActivePlayers;
        }
        set
        {
            iNumberOfActivePlayers = value;
        }
    }

    public static int MaxNumberOfPlayers
    {
        get
        {
            return iMaxNumberOfPlayers;
        }
        set
        {
            iMaxNumberOfPlayers = value;
        }
    }

    public static int MaxNumberOfBalls
    {
        get
        {
            return iMaxNumberOfBalls;
        }
        set
        {
            iMaxNumberOfBalls = value;
        }
    }

    public static int NumberOfBalls
    {
        get
        {
            return iNumberOfBalls;
        }
        set
        {
            iNumberOfBalls = value;
        }
    }

    public static int MaxNumberOfItems
    {
        get
        {
            return iMaxNumberOfItems;
        }
        set
        {
            iMaxNumberOfItems = value;
        }
    }

    public static int NumberOfItems
    {
        get
        {
            return iNumberOfItems;
        }
        set
        {
            iNumberOfItems = value;
        }
    }

    public static int PlayerLevel
    {
        get
        {
            return iPlayerLevel;
        }
        set
        {
            iPlayerLevel = value;
        }
    }

    public static int GameLevel
    {
        get
        {
            return iGameLevel;
        }
        set
        {
            iGameLevel = value;
        }
    }

    public static float CreateItemTimer
    {
        get
        {
            return fCreateItemTimer;
        }
        set
        {
            fCreateItemTimer = value;
        }
    }

    public static int MaxNumberOfTeams
    {
        get
        {
            return iMaxNumberOfTeams;
        }
        set
        {
            iMaxNumberOfTeams = value;
        }
    }

    public static int NumberOfTeams
    {
        get
        {
            return iNumberOfTeams;
        }
        set
        {
            iNumberOfTeams = value;
        }
    }

    public static int NumberOfActiveTeams
    {
        get
        {
            return iNumberOfActiveTeams;
        }
        set
        {
            iNumberOfActiveTeams = value;
        }
    }
    public static void setGameState(State state)
    {
        GameState = state;
    }

    public static State getGameState()
    {
        return GameState;
    }
}
