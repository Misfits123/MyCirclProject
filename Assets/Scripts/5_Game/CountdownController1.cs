using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownController1 : MonoBehaviour
{
    public bool bCountdownRunning = true;
    private AudioManager oAudioManagerComp;
    private float fStartTime = 6.0f;

    void Awake()
    {
        this.GetComponent<Text>().text = CommonValues.sCanvasSuddenDeathText;
        this.GetComponent<Text>().fontSize = CommonValues.iCanvasSuddenDeathTextSize;
    }

    void Start()
    {
        oAudioManagerComp = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bCountdownRunning == true)
        {
            if (fStartTime > 0)
            {
                fStartTime -= Time.deltaTime;
            }
            else
            {
                fStartTime = 0.0f;
                CommonValues.setGameState(CommonValues.State.START);
                this.GetComponent<Animator>().SetTrigger("ClearCountdown");
                //oAudioManagerComp.playGameStart();
                bCountdownRunning = false;
            }

            if (fStartTime > 1)
            {
                if(fStartTime > 4f && fStartTime < 5f)
                {
                    this.GetComponent<Text>().text = CommonValues.sCanvasSuddenDeathText;
                    this.GetComponent<Text>().fontSize = CommonValues.iCanvasSuddenDeathTextSize;
                }
                else if(fStartTime > 2f && fStartTime < 4f)
                {
                    this.GetComponent<Text>().text = "LET'S";
                    this.GetComponent<Text>().fontSize = 200;
                }
                else if(fStartTime > 1f && fStartTime < 2f)
                {
                    this.GetComponent<Text>().text = "CIRCL!";
                }
                else 
                {
                    //do nothing
                }
            }
        }
    }
}
