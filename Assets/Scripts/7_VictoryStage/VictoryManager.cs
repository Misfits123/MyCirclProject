using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public GameObject[] CanvasObjectArray;
    public Material[] PlayerMaterial;

    private AudioManager oAudioManager;
    private ScoreManager2 ScoreManager;
    private GameObject[] Player;
    private float timeLimit = 15.0f;
    private bool bShowInfoOnce = false;

    void Start()
    {
        oAudioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        ScoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager2>();
        oAudioManager.startVictoryMusic();
        CanvasObjectArray[0].gameObject.SetActive(false);
    }
    void Update()
    {
        timeLimit -= Time.deltaTime;
        if(timeLimit <= 0)
        {
            if(!bShowInfoOnce)
            {
                if (CommonValues.bGermanLanguage == false)
                {
                    CanvasObjectArray[0].gameObject.SetActive(true);
                }
                else
                {
                    CanvasObjectArray[1].gameObject.SetActive(true);
                }
                for (int i = 0; i < ScoreManager.oActiveScoreArray.Length; i++)
                {
                    if (ScoreManager.oActiveScoreArray[i] != null)
                    {
                        ScoreManager.oActiveScoreArray[i].gameObject.SetActive(true);
                    }
                }
                bShowInfoOnce = true;
            }

            for (int i = 0; i < CommonValues.MaxNumberOfPlayers; i++)
            {
                if (CirCl.GetButtonDown(i, 0))
                {
                    oAudioManager.playMaskDisapear();
                    CanvasObjectArray[0].gameObject.SetActive(false);
                    CanvasObjectArray[1].gameObject.SetActive(false);
                    timeLimit = 15.0f;
                    StartCoroutine(delayedSceneChange());
                }
            }
        }
        //Vector3 rotationWorld = new Vector3(0f, 10 * Time.deltaTime, 0f);
        //transform.eulerAngles += rotationWorld;
        //Vector3 rotationLocal = new Vector3(0f, 0f, 10 * Time.deltaTime);
        //OrbitCamera.transform.eulerAngles += rotationLocal;
    }

    IEnumerator delayedSceneChange()
    {
        yield return new WaitForSeconds(1.0f);
        oAudioManager.stopVictoryMusic();
        SceneManager.LoadScene(CommonValues.sSceneArray[0]);
    }
}