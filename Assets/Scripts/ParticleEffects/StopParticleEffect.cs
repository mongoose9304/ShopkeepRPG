using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopParticleEffect : MonoBehaviour
{
    public bool destroyOnEnd;
    public float maxTime;
    float currentTime;
    private void OnEnable()
    {
        currentTime = maxTime;
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;
        if(currentTime<=0)
        {
            if(destroyOnEnd)
            {
                Destroy(this.gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
