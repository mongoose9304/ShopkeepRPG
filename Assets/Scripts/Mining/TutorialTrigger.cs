using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
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
