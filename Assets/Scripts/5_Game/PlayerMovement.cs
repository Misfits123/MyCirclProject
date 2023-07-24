using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // public variables
    // ----------------------------------------------------------------------------------------------------------------------------
    public float fSpeed = 15f;            // the speed that the player will move at

    // privat variables
    // ----------------------------------------------------------------------------------------------------------------------------
    private GameObject oPlayerManagerObj;           // reference to PlayerManager object
    private PlayerManager oPlayerManagerComp;       // reference to PlayerManager component
    private GameObject oAreaObj;
    private Vector3[] oAreaArray;
    private Vector3 oMovement;                      // the vector to store the direction of the player's movement
    private Rigidbody oPlayerRigidbody;           // reference to the player's rigidbody
    private int iPlayerIndex = 0;

    void Awake()
    {
        // set up reference
        oPlayerManagerObj = GameObject.Find("PlayerManager");
        oPlayerManagerComp = oPlayerManagerObj.GetComponent<PlayerManager>();
        oPlayerRigidbody = GetComponent<Rigidbody>();

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

        // set up reference
        int i_area_index = iPlayerIndex - 1;
        oAreaObj = GameObject.Find("AreaPlayer" + i_area_index.ToString());
        Matrix4x4 worldCoordinates = oAreaObj.transform.localToWorldMatrix;
        oAreaArray = new Vector3[oAreaObj.GetComponent<MeshFilter>().mesh.vertices.Length];
        for (int i = 0; i < oAreaArray.Length; i++)
        {
            oAreaArray[i] = worldCoordinates.MultiplyPoint3x4(oAreaObj.GetComponent<MeshFilter>().mesh.vertices[i]);
        }
    }

    void FixedUpdate()
    {
        //if player out of bounds
        if (CommonMethods.isPointInPolygon(transform.position, oAreaArray) == false)
        {
            // set the player to start position
            transform.position = oPlayerManagerComp.oPlayerSpawnPointArray[iPlayerIndex - 1].position;
        }

        // store the input axes
        float f_horizontal = 0f;
        float f_vertical = 0f;
        f_horizontal = CirCl.GetAxis(iPlayerIndex - 1, 'h', false);
        f_vertical = CirCl.GetAxis(iPlayerIndex - 1, 'v', false);
        decimal d_horizontal = Math.Round((decimal)f_horizontal, 1);
        decimal d_vertical = Math.Round((decimal)f_vertical, 1);
        MoveNew((float)d_horizontal, (float)d_vertical);
    }

    void MoveNew(float h, float v)
    {
        // set the movement vector based on the axis input
        oMovement.Set(h, 0f, v);

        // normalise the movement vector and make it proportional to the speed per second
        oMovement = oMovement.normalized * fSpeed * Time.deltaTime;

        // XXX_SpeciallyForCircl_XXX: rotate the movement vector according to the player perspective because player sitting in a circle
        float f_player_viewpoint = CommonValues.fPlayerViewpointArray[iPlayerIndex - 1];
        Vector3 o_default_move_vector = oMovement;
        Vector3 o_rotation_vector = new Vector3(0f, 1f, 0f);
        Quaternion o_player_rotation = Quaternion.AngleAxis(f_player_viewpoint, o_rotation_vector);
        Vector3 o_player_move_vector_3d = o_player_rotation * o_default_move_vector;

        // move the player to it's current position plus the movement
        Vector3 o_move_3d = transform.position + o_player_move_vector_3d;

        // check that the next move is indside of this area
        //bool b_inside = false;
        //if(o_move_3d.x >= oAreaObj.transform.position.x - 3f && o_move_3d.x <= oAreaObj.transform.position.x + 3f)
        //{
        //    if(o_move_3d.z >= oAreaObj.transform.position.z - 5.5f && o_move_3d.z <= oAreaObj.transform.position.z + 5.5f)
        //    {
        //        b_inside = true;
        //    }
        //}
        bool b_inside = CommonMethods.isPointInPolygon(o_move_3d, oAreaArray);
        if (b_inside && CommonValues.getGameState() == CommonValues.State.PLAY)
        {
            // if yes move the player
            oPlayerRigidbody.MovePosition(o_move_3d);
        }
        else
        {
            oPlayerRigidbody.MovePosition(transform.position);
        }
    }
}