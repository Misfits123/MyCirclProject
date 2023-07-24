using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    // public variables
    // ----------------------------------------------------------------------------------------------------------------------------
    public float fSpeed;            // The speed that the ball will move at.
    public bool bKickOff;
    public Vector3 oVelocity;
    private AudioSource[] hitSounds;
    private GameObject oAreaObj;
    private Vector3[] oAreaArray;

    void Start()
    {
        bKickOff = false;
        hitSounds = transform.GetComponents<AudioSource>();

        oAreaObj = GameObject.Find("Area");
        Matrix4x4 worldCoordinates = oAreaObj.transform.localToWorldMatrix;
        oAreaArray = new Vector3[oAreaObj.GetComponent<MeshFilter>().mesh.vertices.Length];
        for (int i = 0; i < oAreaArray.Length; i++)
        {
            oAreaArray[i] = worldCoordinates.MultiplyPoint3x4(oAreaObj.GetComponent<MeshFilter>().mesh.vertices[i]);
        }
    }
    void Update()
    {
        oVelocity = gameObject.GetComponent<Rigidbody>().velocity;
        fSpeed = Mathf.Sqrt(Mathf.Pow(oVelocity.x, 2) + Mathf.Pow(oVelocity.z, 2));
        if (fSpeed > 3f)
        {
            bKickOff = true;
        }
    }

    Vector3 calculateHitFactor(Transform o_ball_pos, Transform o_player_collision_pos, Bounds o_collider_size)
    {
        float f_hit_factor_x = (o_ball_pos.position.x - o_player_collision_pos.position.x) / o_collider_size.size.x;
        float f_hit_factor_z = (o_ball_pos.position.z - o_player_collision_pos.position.z) / o_collider_size.size.z;
        Vector3 o_hit_vector = new Vector3(f_hit_factor_x, 0f, f_hit_factor_z).normalized;
        return o_hit_vector;
    }

    Vector3 calculateHitFactorWall(Transform o_ball_pos, Transform o_player_collision_pos, Bounds o_collider_size)
    {
        float f_hit_factor_x = (o_ball_pos.position.x - o_player_collision_pos.position.x) / o_collider_size.size.x;
        float f_hit_factor_z = (o_ball_pos.position.z - o_player_collision_pos.position.z) / o_collider_size.size.z;
        Vector3 o_hit_vector = new Vector3(f_hit_factor_x, 0f, f_hit_factor_z).normalized;
        return o_hit_vector;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            // play sound
            hitSounds[0].Play();

            // calculate hit factor
            Vector3 o_dir_vector = calculateHitFactor(transform, collision.transform, collision.collider.bounds);

            try
            {
                gameObject.GetComponent<Rigidbody>().velocity = o_dir_vector * fSpeed;
            }
            catch (System.Exception)
            {

            }
        }
    }
}
