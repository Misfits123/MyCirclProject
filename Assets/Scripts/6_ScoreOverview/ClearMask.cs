using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearMask : StateMachineBehaviour
{
    // private variables
    // ----------------------------------------------------------------------------------------------------------------------------
    private GameObject victoryPanelObj;
    private AudioManager audioManagerComp;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // set references
        victoryPanelObj = GameObject.Find("VictoryPanel");
        audioManagerComp = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        // set menu buttons active
        CommonValues.bSceneInfoArray[6] = true;

        // play game menu music
        audioManagerComp.startGameMenuMusic2();

        // set obj inactive
        victoryPanelObj.SetActive(false);
    }
}
