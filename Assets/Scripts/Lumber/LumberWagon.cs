using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LumberWagon : MonoBehaviour
{
    public UnityEvent endTutEvent;
   public void ActivateWagon()
    {
        LumberLevelManager.instance.WinLevel();
    }
    public void EndTutorial()
    {
        endTutEvent.Invoke();
    }
}
