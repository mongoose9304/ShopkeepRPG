using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ChracterStun : MonoBehaviour
{
    float timeRemaining;
    public UnityEvent Start;
    public UnityEvent End;

    public void ApplyStun(float stunDuration) {
        //Stun code
        timeRemaining = stunDuration;
        Stun();
        Start.Invoke(); 
    }
    public void Stun() {
        //stun code
    }

    public void Update() { 
        timeRemaining -= Time.deltaTime;
        if(timeRemaining <= 0) {
            End.Invoke();
        }
    }
}