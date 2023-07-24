using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    // public variables
    // ----------------------------------------------------------------------------------------------------------------------------
    public GameObject[] oItemArray;

    // privat variables
    // ----------------------------------------------------------------------------------------------------------------------------
    GameObject oItemObj;
    GameObject oBallObj;
    BallMovement oBallComp;
    GameObject[] oActiveItemArray;
    GameObject oAreaObj;
    Vector3[] oAreaArray;
    float fTimer = 0f;
    string[] sItemNameArray = { "Item1", "Item2", "Item3", "Item4", "Item5", "Item6", "Item7", "Item8" };
    GameObject oBallManagerObj;
    BallManager oBallManagerComp;
    GameObject oPlayerManager;
    PlayerManager oPlayerManagerComp;

    void Start()
    {
        //set init values
        fTimer = CommonValues.CreateItemTimer;
        oActiveItemArray = new GameObject[CommonValues.MaxNumberOfItems];

        // set up the reference.
        oBallManagerObj = GameObject.Find("BallManager");
        oBallManagerComp = oBallManagerObj.GetComponent<BallManager>();

        // set up the reference.
        oPlayerManager = GameObject.Find("PlayerManager");
        oPlayerManagerComp = oPlayerManager.GetComponent<PlayerManager>();

        // set up reference
        oAreaObj = GameObject.Find("AreaItem");
        Matrix4x4 worldCoordinates = oAreaObj.transform.localToWorldMatrix;
        oAreaArray = new Vector3[oAreaObj.GetComponent<MeshFilter>().mesh.vertices.Length];
        for (int i = 0; i < oAreaArray.Length; i++)
        {
            oAreaArray[i] = worldCoordinates.MultiplyPoint3x4(oAreaObj.GetComponent<MeshFilter>().mesh.vertices[i]);
        }

        addItem();
    }

    void Update()
    {
        // reduce timer
        fTimer -= Time.deltaTime;

        // if timer equal 0 create a new item
        if (fTimer <= 0f)
        {
            // reset events
            resetEvents();

            // create new item
            addItem();

            // reset timer
            fTimer = CommonValues.CreateItemTimer;
        }

        // check collision of every ball with every item
        detectCollision();

        // one ball is out of playing area
        if(CommonValues.getGameState() == CommonValues.State.PLAY)
        {
            for (int i = 0; i < CommonValues.NumberOfBalls; i++)
            {
                oBallObj = oBallManagerComp.oActiveBallArray[i];
                bool b_inside = false;
                if (oBallObj.transform.position.x >= oAreaObj.transform.position.x - 14f && oBallObj.transform.position.x <= oAreaObj.transform.position.x + 14f)
                {
                    if (oBallObj.transform.position.z >= oAreaObj.transform.position.z - 14f && oBallObj.transform.position.z <= oAreaObj.transform.position.z + 14f)
                    {
                        b_inside = true;
                    }
                }
                if (!b_inside || (Math.Round(oBallObj.GetComponent<Rigidbody>().velocity.magnitude, 1) < 3f && oBallObj.GetComponent<BallMovement>().bKickOff == true))
                {
                    oBallManagerComp.deleteBall(i);
                    oBallManagerComp.addBall(i);
                }
            }
        }
    }

    public void addItem()
    {
        if (CommonValues.NumberOfItems < CommonValues.MaxNumberOfItems)
        {
            // fill active item array
            bool b_add_once = false;
            for (int i = 0; i < CommonValues.MaxNumberOfItems; i++)
            {
                if(oActiveItemArray[i] == null && b_add_once == false)
                {
                    CommonValues.NumberOfItems += 1;
                    Vector3 o_start_position = CommonMethods.createRandomPositionInArea(oAreaObj.transform.position, 5f) ;
                    Quaternion o_start_rotation = new Quaternion();
                    System.Random oRandom = new System.Random();
                    int i_rnd_index = oRandom.Next(0, CommonValues.MaxNumberOfItems);//oItemArray.Length);
                    oActiveItemArray[i] = Instantiate(oItemArray[i_rnd_index], o_start_position, o_start_rotation);
                    oActiveItemArray[i].name = sItemNameArray[i_rnd_index] + "_" + i.ToString();
                    b_add_once = true;
                }
            }
        }
    }

    public void detectCollision()
    {
        for(int i = 0; i < CommonValues.MaxNumberOfBalls; i++)
        {
            if (oBallManagerComp.oActiveBallArray[i] != null)
            {
                // select ball object
                oBallObj = oBallManagerComp.oActiveBallArray[i];

                for (int j = 0; j < CommonValues.MaxNumberOfItems; j++)
                {
                    if (oActiveItemArray[j] != null)
                    {
                        // define item area
                        float f_item_range_x = 1f;
                        float f_item_range_z = 1f;
                        Vector3 o_point1 = new Vector3(oActiveItemArray[j].transform.position.x - f_item_range_x, 0f, oActiveItemArray[j].transform.position.z - f_item_range_z);
                        Vector3 o_point2 = new Vector3(oActiveItemArray[j].transform.position.x - f_item_range_x, 0f, oActiveItemArray[j].transform.position.z + f_item_range_z);
                        Vector3 o_point3 = new Vector3(oActiveItemArray[j].transform.position.x + f_item_range_x, 0f, oActiveItemArray[j].transform.position.z + f_item_range_z);
                        Vector3 o_point4 = new Vector3(oActiveItemArray[j].transform.position.x + f_item_range_x, 0f, oActiveItemArray[j].transform.position.z - f_item_range_z);
                        Vector3[] o_item_area_array = new Vector3[] { o_point1, o_point2, o_point3, o_point4 };

                        // check that the ball is indside of the item area.
                        bool b_inside = CommonMethods.isPointInPolygon(oBallObj.transform.position, o_item_area_array);
                        if (b_inside == true)
                        {
                            // trigger event
                            triggerEvent(oActiveItemArray[j].name, oBallObj);

                            // reduce number of items
                            CommonValues.NumberOfItems -= 1;

                            // delete detect item
                            Destroy(oActiveItemArray[j]);
                        }
                    }
                }
            }
        }
    }

    public void triggerEvent(string s_item_name, GameObject o_ball_obj)
    {
        // item 0 --> scale the detect ball to the half
        if(s_item_name.Contains(sItemNameArray[0]))
        {
            Vector3 o_scale_vector = new Vector3(0.6f, 1.0f, 0.6f);
            o_ball_obj.GetComponent<Transform>().transform.localScale = o_scale_vector;
        }

        // item 1 --> scale all player to the half
        if (s_item_name.Contains(sItemNameArray[1]))
        {
            for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
            {
                if (oPlayerManagerComp.oActivePlayerArray[i] != null)
                {
                    Vector3 o_scale_vector = new Vector3(2.5f, 1.0f, 1.0f);
                    oPlayerManagerComp.oActivePlayerArray[i].GetComponent<Transform>().transform.localScale = o_scale_vector;
                }
            }
        }

        // item 2 --> add a new ball
        if (s_item_name.Contains(sItemNameArray[2]))
        {
            oBallManagerComp.addBall(CommonValues.NumberOfBalls);
        }
    }

    public void resetEvents()
    {
        for (int i = 0; i < CommonValues.MaxNumberOfBalls; i++)
        {
            if (oBallManagerComp.oActiveBallArray[i] != null)
            {
                Vector3 o_scale_vector = new Vector3(1.1f, 1.0f, 1.1f);
                oBallObj.GetComponent<Transform>().transform.localScale = o_scale_vector;
            }
        }

        for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
        {
            if (oPlayerManagerComp.oActivePlayerArray[i] != null)
            {
                Vector3 o_scale_vector = new Vector3(5.0f, 1.0f, 1.0f);
                oPlayerManagerComp.oActivePlayerArray[i].GetComponent<Transform>().transform.localScale = o_scale_vector;
            }
        }
    }
}