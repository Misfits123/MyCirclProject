using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasResolutionController : MonoBehaviour {

    public GameObject[] deactivateObjectArray;
    public GameObject[] moveObjectArray;
    public GameObject[] changeTextObjectArray;

    void Awake()
    {
        float aspect_ratio = (float)Screen.width / (float)Screen.height;
        //aspect_ratio = 2f;
        //for 12:9 devices
        if (aspect_ratio < 1.5f)
        {
            for (int i = 0; i < moveObjectArray.Length; i++)
            {
                Vector3 move_vec3 = new Vector3(0, 0, 0);
                if (i < 2)
                {
                    move_vec3 = new Vector3(80, 0, 0);
                    moveObjectArray[i].transform.position += move_vec3;
                }
                else if (i < 4)
                {
                    move_vec3 = new Vector3(80, 0, 0);
                    moveObjectArray[i].transform.position += move_vec3;
                }
                else if (i < 6)
                {
                    move_vec3 = new Vector3(-80, 0, 0);
                    moveObjectArray[i].transform.position += move_vec3;
                }
                else if (i < 8)
                {
                    move_vec3 = new Vector3(-80, 0, 0);
                    moveObjectArray[i].transform.position += move_vec3;
                }
            }
            foreach (GameObject obj in changeTextObjectArray)
            {
                if (obj.name == "GameInfo")
                {
                    //obj.transform.GetComponent<Text>().fontSize = 65;
                    Vector3 move_vec3 = new Vector3(0, -100, 0);
                    obj.transform.position += move_vec3;
                    //obj.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 450);
                }
                if (obj.name == "GameMusic")
                {
                    //obj.transform.GetComponent<Text>().fontSize = 65;
                    Vector3 move_vec3 = new Vector3(0, -100, 0);
                    obj.transform.position += move_vec3;
                    //obj.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 700);
                }
                if (obj.name == "GamePlayer")
                {
                    //obj.transform.GetComponent<Text>().fontSize = 65;
                    Vector3 move_vec3 = new Vector3(0, -25, 0);
                    obj.transform.position += move_vec3;
                    //obj.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 900);
                }
                if (obj.name == "RoundsText")
                {
                    obj.transform.GetComponent<Text>().fontSize = 25;
                }
                if (obj.name == "RoundsText2")
                {
                    obj.transform.GetComponent<Text>().fontSize = 25;
                    Vector3 move_vec3 = new Vector3(25, 0, 0);
                    obj.transform.position += move_vec3;
                }
                if (obj.name == "RoundsText3")
                {
                    obj.transform.GetComponent<Text>().fontSize = 25;
                }
                if (obj.name == "RoundsText4")
                {
                    obj.transform.GetComponent<Text>().fontSize = 25;
                    Vector3 move_vec3 = new Vector3(-25, 0, 0);
                    obj.transform.position += move_vec3;
                }
            }
            if (SceneManager.GetActiveScene().name == CommonValues.sSceneArray[5])
            {
                this.GetComponent<CanvasScaler>().referenceResolution = new Vector2(2000, 1000);
            }
            else
            {
                this.GetComponent<CanvasScaler>().referenceResolution = new Vector2(800, 700);
            }
        }
        //for 18:9 devices
        else if (aspect_ratio > 1.9f)
        {
            if (SceneManager.GetActiveScene().name == CommonValues.sSceneArray[5])
            {
                this.GetComponent<CanvasScaler>().referenceResolution = new Vector2(2600, 1000);
            }
            else
            {
                this.GetComponent<CanvasScaler>().referenceResolution = new Vector2(800, 600);
            }
        }
        //for 16:9 devices
        else
        {
            if (SceneManager.GetActiveScene().name == CommonValues.sSceneArray[5])
            {
                this.GetComponent<CanvasScaler>().referenceResolution = new Vector2(2200, 1000);
            }
            else
            {
                this.GetComponent<CanvasScaler>().referenceResolution = new Vector2(800, 600);
            }
        }
    }
}
