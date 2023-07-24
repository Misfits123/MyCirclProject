using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

public class CommonMethods : MonoBehaviour
{
    public static Vector3 createRandomPositionInArea(Vector3 center, float size)
    {
        // input
        Vector3 o_rnd_vector = new Vector3();
        System.Random oRandom = new System.Random();

        // calculate random value in this area
        int i_area_position_x = (int)Math.Round(center.x, 0);
        int i_area_position_y = 0;
        int i_area_position_z = (int)Math.Round(center.z, 0);
        i_area_position_x = oRandom.Next(i_area_position_x - (int)size, i_area_position_x + (int)size);
        i_area_position_y = 0;
        i_area_position_z = oRandom.Next(i_area_position_z - (int)size, i_area_position_z + (int)size);

        // output
        o_rnd_vector.Set(i_area_position_x, i_area_position_y, i_area_position_z);
        return o_rnd_vector;
    }
    public static bool isPointInPolygon(Vector3 point, Vector3[] polygon)
    {
        //input
        int i_poly_length = polygon.Length, i = 0;
        bool b_inside = false;
        // x, z for tested point
        float f_point_x = point.x;
        float f_point_z = point.z;

        // start / end point for the current polygon segment
        float f_start_x, f_start_z, f_end_x, f_end_z;
        Vector3 endPoint = polygon[i_poly_length - 1];
        f_end_x = endPoint.x;
        f_end_z = endPoint.z;
        while (i < i_poly_length)
        {
            f_start_x = f_end_x; f_start_z = f_end_z;
            endPoint = polygon[i++];
            f_end_x = endPoint.x; f_end_z = endPoint.z;
            b_inside ^= (f_end_z > f_point_z ^ f_start_z > f_point_z) && ((f_point_x - f_end_x) < (f_point_z - f_end_z) * (f_start_x - f_end_x) / (f_start_z - f_end_z));
        }

        //output
        return b_inside;
    }
    public static Vector3 rotateMovementAccordingToPlayerPerspective(int i_player_index, Vector3 o_movement_vec, Vector3 o_rotation_vec)
    {
        // rotate the movement vector according to the player perspective
        float f_player_viewpoint = CommonValues.fPlayerViewpointArray[i_player_index - 1];
        Vector3 o_default_move_vector = o_movement_vec;
        Quaternion o_player_rotation = Quaternion.AngleAxis(f_player_viewpoint, o_rotation_vec);
        Vector3 o_player_move_vector_3d = o_player_rotation * o_default_move_vector;

        return o_player_move_vector_3d;
    }
    public static int[,] countRankings()
    {
        int[,] rankings = new int[3, CommonValues.MaxNumberOfPlayers + 2];
        int first_place_counter = 0;
        int second_place_counter = 0;
        int third_place_counter = 0;
        if(CommonValues.GameMode == 1)
        {
            for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
            {
                if (CommonValues.PlayerParameterArray[i].score_ranking == 1)
                {
                    rankings[0, first_place_counter] = CommonValues.PlayerParameterArray[i].index;
                    first_place_counter += 1;
                }
                else if (CommonValues.PlayerParameterArray[i].score_ranking == 2)
                {
                    rankings[1, second_place_counter] = CommonValues.PlayerParameterArray[i].index;
                    second_place_counter += 1;
                }
                else if (CommonValues.PlayerParameterArray[i].score_ranking == 3)
                {
                    rankings[2, third_place_counter] = CommonValues.PlayerParameterArray[i].index;
                    third_place_counter += 1;
                }
                else
                {
                    // do nothing
                }
            }
        }
        else
        {
            List<int> i_first_list = new List<int>();
            List<int> i_second_list = new List<int>();
            List<int> i_third_list = new List<int>();
            for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
            {
                if (CommonValues.PlayerParameterArray[i].team_ranking == 1)
                {
                    rankings[0, first_place_counter] = CommonValues.PlayerParameterArray[i].index;
                    if(i_first_list.Contains(CommonValues.PlayerParameterArray[i].team) == false)
                    {
                        i_first_list.Add(CommonValues.PlayerParameterArray[i].team);
                    }
                    first_place_counter += 1;
                }
                else if (CommonValues.PlayerParameterArray[i].team_ranking == 2)
                {
                    rankings[1, second_place_counter] = CommonValues.PlayerParameterArray[i].index;
                    if (i_second_list.Contains(CommonValues.PlayerParameterArray[i].team) == false)
                    {
                        i_second_list.Add(CommonValues.PlayerParameterArray[i].team);
                    }
                    second_place_counter += 1;
                }
                else if (CommonValues.PlayerParameterArray[i].team_ranking == 3)
                {
                    rankings[2, third_place_counter] = CommonValues.PlayerParameterArray[i].index;
                    rankings[1, second_place_counter] = CommonValues.PlayerParameterArray[i].index;
                    if (i_third_list.Contains(CommonValues.PlayerParameterArray[i].team) == false)
                    {
                        i_third_list.Add(CommonValues.PlayerParameterArray[i].team);
                    }
                    third_place_counter += 1;
                }
                else
                {
                    // do nothing
                }
            }
            rankings[0, CommonValues.MaxNumberOfPlayers + 1] = i_first_list.Count;
            rankings[1, CommonValues.MaxNumberOfPlayers + 1] = i_second_list.Count;
            rankings[2, CommonValues.MaxNumberOfPlayers + 1] = i_third_list.Count;
        }
        rankings[0, CommonValues.MaxNumberOfPlayers] = first_place_counter;
        rankings[1, CommonValues.MaxNumberOfPlayers] = second_place_counter;
        rankings[2, CommonValues.MaxNumberOfPlayers] = third_place_counter;
        return rankings;
    }
    public static void setPlayerRanking()
    {
        for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
        {
            if (CommonValues.PlayerParameterArray[i].mode == 1 || CommonValues.PlayerParameterArray[i].mode == 3)
            {
                int i_rank = CommonValues.NumberOfPlayers;
                int i_score = CommonValues.PlayerParameterArray[i].score;
                for (int j = 0; j < CommonValues.MaxNumberOfPlayers; j++)
                {
                    // just the active player
                    if (CommonValues.PlayerParameterArray[j].mode == 1 || CommonValues.PlayerParameterArray[j].mode == 3)
                    {
                        if (i_score > CommonValues.PlayerParameterArray[j].score)
                        {
                            i_rank -= 1;
                        }
                    }
                }
                CommonValues.PlayerParameterArray[i].score_ranking = i_rank;
            }
        }
    }
    public static void setTeamRanking()
    {
        // calculate team score for each player
        for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
        {
            // just the active players
            if (CommonValues.PlayerParameterArray[i].mode == 1 || CommonValues.PlayerParameterArray[i].mode == 3)
            {
                // get team of current player
                int i_team = CommonValues.PlayerParameterArray[i].team;

                // increase the current team score
                int i_team_score = 0;
                for (int j = 0; j < CommonValues.MaxNumberOfPlayers; j++)
                {
                    //sum all scores from the same team
                    if (CommonValues.PlayerParameterArray[j].team == i_team && i_team > 0)
                    {
                        i_team_score += CommonValues.PlayerParameterArray[j].death_ranking;
                        if (CommonValues.TeamParameterArray[i_team - 1].active_member > 1)
                        {
                            i_team_score += CommonValues.TeamParameterArray[i_team - 1].active_member - 1;
                        }
                    }
                }
                if (i_team_score > 0 && i_team > 0)
                {
                    i_team_score = (int)Mathf.Round(i_team_score / CommonValues.TeamParameterArray[i_team - 1].member);
                }
                CommonValues.PlayerParameterArray[i].team_score += i_team_score;
                CommonValues.PlayerParameterArray[i].team_score_last_game = i_team_score;
            }
        }

        for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
        {
            // just the active players
            if (CommonValues.PlayerParameterArray[i].mode == 1 || CommonValues.PlayerParameterArray[i].mode == 3)
            {
                // set team ranking
                int i_team_rank = CommonValues.NumberOfTeams;
                int i_team_rank_last_game = CommonValues.NumberOfTeams;
                int i_team_score = CommonValues.PlayerParameterArray[i].team_score;
                int i_team_score_last_game = CommonValues.PlayerParameterArray[i].team_score_last_game;
                List<int> i_team_list = new List<int>();
                List<int> i_team_last_game_list = new List<int>();
                for (int j = 0; j < CommonValues.MaxNumberOfPlayers; j++)
                {
                    if (i_team_score > CommonValues.PlayerParameterArray[j].team_score && i_team_list.Contains(CommonValues.PlayerParameterArray[j].team) == false && CommonValues.PlayerParameterArray[j].team > 0)
                    {
                        i_team_rank -= 1;
                        i_team_list.Add(CommonValues.PlayerParameterArray[j].team);
                    }
                    if (i_team_score_last_game > CommonValues.PlayerParameterArray[j].team_score_last_game && i_team_last_game_list.Contains(CommonValues.PlayerParameterArray[j].team) == false && CommonValues.PlayerParameterArray[j].team > 0)
                    {
                        i_team_rank_last_game -= 1;
                        i_team_last_game_list.Add(CommonValues.PlayerParameterArray[j].team);
                    }
                }
                CommonValues.PlayerParameterArray[i].team_ranking = i_team_rank;
                CommonValues.PlayerParameterArray[i].team_ranking_last_game = i_team_rank_last_game;
            }
        }
    }
    public static void countNumberOfPlayer()
    {
        // count number of player
        CommonValues.NumberOfPlayers = 0;
        for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
        {
            if (CommonValues.PlayerParameterArray[i].mode == 1)
            {
                CommonValues.NumberOfPlayers += 1;
            }
        }
    }
    public static void countNumberOfActivePlayer()
    {
        // count number of active player
        CommonValues.NumberOfActivePlayers = 0;
        for (int i = 0; i < CommonValues.PlayerParameterArray.GetLength(0); i++)
        {
            if (CommonValues.PlayerParameterArray[i].state != 0)
            {
                CommonValues.NumberOfActivePlayers += 1;
            }
        }
    }
    public static void countNumberOfTeams()
    {
        // count number of teams
        for (int i = 0; i < CommonValues.MaxNumberOfTeams; i++)
        {
            CommonValues.TeamParameterArray[i].member = 0;
            CommonValues.TeamParameterArray[i].active_member = 0;
        }

        // set team parameter
        for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
        {
            if (CommonValues.PlayerParameterArray[i].team == 1)
            {
                CommonValues.TeamParameterArray[0].member += 1;
            }
            else if (CommonValues.PlayerParameterArray[i].team == 2)
            {
                CommonValues.TeamParameterArray[1].member += 1;
            }
            else if (CommonValues.PlayerParameterArray[i].team == 3)
            {
                CommonValues.TeamParameterArray[2].member += 1;
            }
            else if (CommonValues.PlayerParameterArray[i].team == 4)
            {
                CommonValues.TeamParameterArray[3].member += 1;
            }
            else
            {
                //do nothing
            }
        }

        // reset number of teams
        CommonValues.NumberOfTeams = 0;

        // set number of teams
        for (int i = 0; i < CommonValues.TeamParameterArray.GetLength(0); i++)
        {
            if (CommonValues.TeamParameterArray[i].member != 0)
            {
                CommonValues.NumberOfTeams += 1;
            }
        }
    }
    public static void countNumberOfActiveTeams()
    {
        // count number of active teams
        CommonValues.NumberOfActiveTeams = 0;
        for (int i = 0; i < CommonValues.TeamParameterArray.GetLength(0); i++)
        {
            if (CommonValues.TeamParameterArray[i].active_member != 0)
            {
                CommonValues.NumberOfActiveTeams += 1;
            }
        }
    }
    public static void checkGameIsOver()
    {
        if (CommonValues.GameMode == 1)
        {
            // count number of active player
            countNumberOfActivePlayer();

            // if the number of deaths is one lower then the number of players
            if (CommonValues.NumberOfActivePlayers == 1 && CommonValues.getGameState() == CommonValues.State.PLAY)
            {
                // set game over state
                CommonValues.setGameState(CommonValues.State.GAMEOVER);

                // set the score of the last player
                for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
                {
                    // if player is alive
                    if (CommonValues.PlayerParameterArray[i].state == 1)
                    {
                        // set player score
                        CommonValues.PlayerParameterArray[i].score += CommonValues.iNumberOfDeaths;

                        // set player death ranking
                        CommonValues.PlayerParameterArray[i].death_ranking = CommonValues.iNumberOfDeaths;

                        // set ranking of last match
                        CommonValues.PlayerParameterArray[i].ranking_last_game = CommonValues.NumberOfPlayers - CommonValues.iNumberOfDeaths;
                    }
                }
                // reset number of deaths
                CommonValues.iNumberOfDeaths = 0;
            }
            // player died to the same time
            else if (CommonValues.NumberOfActivePlayers == 0 && CommonValues.getGameState() == CommonValues.State.PLAY)
            {
                // set drawn state
                CommonValues.setGameState(CommonValues.State.DRAWN);

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
            }
            else
            {
                // useful for other game states
            }
        }
        else if (CommonValues.GameMode == 2)
        {
            // count number of active teams
            countNumberOfActiveTeams();

            if (CommonValues.NumberOfActiveTeams == 1 && CommonValues.getGameState() == CommonValues.State.PLAY)
            {
                // set game over state
                CommonValues.setGameState(CommonValues.State.GAMEOVER);

                // set the score of the last player
                for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
                {
                    // if player is alive
                    if (CommonValues.PlayerParameterArray[i].state == 1)
                    {
                        // set player score
                        CommonValues.PlayerParameterArray[i].score += CommonValues.iNumberOfDeaths;

                        // set player death ranking
                        CommonValues.PlayerParameterArray[i].death_ranking = CommonValues.iNumberOfDeaths;

                        // set ranking of last match
                        CommonValues.PlayerParameterArray[i].ranking_last_game = CommonValues.NumberOfPlayers - CommonValues.iNumberOfDeaths;
                    }
                }
                // reset number of deaths
                CommonValues.iNumberOfDeaths = 0;
            }
            // teams died to the same time
            else if (CommonValues.NumberOfActiveTeams == 0 && CommonValues.getGameState() == CommonValues.State.PLAY)
            {
                // set drawn state
                CommonValues.setGameState(CommonValues.State.DRAWN);

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
