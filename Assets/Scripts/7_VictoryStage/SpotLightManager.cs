using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightManager : MonoBehaviour
{
    private GameObject[] spotLights;
    // Start is called before the first frame update
    void Start()
    {
        spotLights = new GameObject[4];
        spotLights[0] = transform.Find("SpotLight1").gameObject;
        spotLights[1] = transform.Find("SpotLight2").gameObject;
        spotLights[2] = transform.Find("SpotLight3").gameObject;
        spotLights[3] = transform.Find("SpotLight4").gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 rotationVector = new Vector3(0f, 30 * Time.deltaTime, 0f);
        spotLights[0].transform.eulerAngles += rotationVector;
        spotLights[1].transform.eulerAngles += rotationVector;
        spotLights[2].transform.eulerAngles += rotationVector;
        spotLights[3].transform.eulerAngles += rotationVector;
    }
}
