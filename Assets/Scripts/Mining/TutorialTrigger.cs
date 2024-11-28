using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets the tutorial to the next state when entered 
/// </summary>
public class TutorialTrigger : MonoBehaviour
{
    [Tooltip("The state to enter")]
    public int tState;
    public bool useAltState;
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag=="Player"|| other.tag == "PlayerFamiliar")
        {
            if(!useAltState)
                TutorialManager.instance_.SetTutorialState(tState);
            else
                TutorialManager.instance_.SetAltTutorialState(tState);
            gameObject.SetActive(false);
        }
    }
}
