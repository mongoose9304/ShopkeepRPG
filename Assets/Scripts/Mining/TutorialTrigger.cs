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
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag=="Player")
        {
            TutorialManager.instance_.SetTutorialState(tState);
            gameObject.SetActive(false);
        }
    }
}
