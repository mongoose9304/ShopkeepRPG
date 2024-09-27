using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialRoom : BasicRoom
{
    public float maxDeadEnemies;
    float currentDeadEnemies;
    public UnityEvent endEvent;
    public void DeadEnemy()
    {
        currentDeadEnemies += 1;
        if (currentDeadEnemies >= maxDeadEnemies)
        {
            endEvent.Invoke();
        }
    }
 
}
