using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BallManager : MonoBehaviour
{
    // public variables
    // ----------------------------------------------------------------------------------------------------------------------------
    public GameObject[] oBallArray;
    public GameObject[] oActiveBallArray = new GameObject[CommonValues.MaxNumberOfBalls];
    public BallMovement[] oActiveBallMovementCompArray = new BallMovement[CommonValues.MaxNumberOfBalls];

    // privat variables
    // ----------------------------------------------------------------------------------------------------------------------------
    GameObject oAreaObj;
    Vector3[] oAreaArray;
    string sBallName = "Ball";

    private void Start()
    {
        // set up reference
        for(int i = 0; i < CommonValues.PlayerParameterArray.Length; i++)
        {
            if(CommonValues.PlayerParameterArray[i].state == 1)
            {
                int player_index = CommonValues.PlayerParameterArray[i].index - 1;
                oAreaObj = GameObject.Find("AreaPlayer" + player_index.ToString());
                break;
            }
        }
        Matrix4x4 worldCoordinates = oAreaObj.transform.localToWorldMatrix;
        oAreaArray = new Vector3[oAreaObj.GetComponent<MeshFilter>().mesh.vertices.Length];
        for (int i = 0; i < oAreaArray.Length; i++)
        {
            oAreaArray[i] = worldCoordinates.MultiplyPoint3x4(oAreaObj.GetComponent<MeshFilter>().mesh.vertices[i]);
        }
        addBall(0);
    }

    public void deleteBall(int ball_id)
    {
        CommonValues.NumberOfBalls -= 1;
        Destroy(oActiveBallArray[ball_id]);
        oActiveBallArray[ball_id] = null;
    }

    public void addBall(int ball_id)
    {
        if (CommonValues.NumberOfBalls < CommonValues.MaxNumberOfBalls)
        {
            for (int i = 0; i < CommonValues.PlayerParameterArray.Length; i++)
            {
                if (CommonValues.PlayerParameterArray[i].state == 1)
                {
                    int player_index = CommonValues.PlayerParameterArray[i].index - 1;
                    oAreaObj = GameObject.Find("AreaPlayer" + player_index.ToString());
                    break;
                }
            }
            CommonValues.NumberOfBalls += 1;
            Vector3 o_start_position = CommonMethods.createRandomPositionInArea(oAreaObj.transform.position, 1f);
            Quaternion o_start_rotation = new Quaternion();
            oActiveBallArray[ball_id] = Instantiate(oBallArray[0], o_start_position, o_start_rotation);
            oActiveBallArray[ball_id].name = sBallName + "_" + ball_id.ToString();
            oActiveBallMovementCompArray[ball_id] = oActiveBallArray[ball_id].GetComponent<BallMovement>();
        }
    }
}