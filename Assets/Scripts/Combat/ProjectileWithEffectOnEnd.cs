using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWithEffectOnEnd : MonoBehaviour
{
    public GameObject objectToSpawn;
    public List<string> activatableTags=new List<string>();

    public virtual void Activate()
    {
        objectToSpawn.transform.position = transform.position;
        objectToSpawn.SetActive(true);
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        foreach(string s_ in activatableTags)
        {
            if(s_==other.tag)
            {
                Activate();
                gameObject.SetActive(false);
            }
        }
    }
}
