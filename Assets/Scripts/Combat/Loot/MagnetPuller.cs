using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPuller : MonoBehaviour
{
    public string[] pullableTags;
    public float pullSpeed;
    private Vector3 velocity = Vector3.zero;
    public float dampModifier;
    private List<GameObject>objectsToPull =new List<GameObject>();
    private void OnTriggerEnter(Collider other)
    {
        foreach(string tag_ in pullableTags)
        {
            if(other.tag==tag_)
            {
                objectsToPull.Add(other.gameObject);
                 //  Vector3 temp = Vector3.Lerp(other.transform.position, this.transform.position, pullSpeed * Time.deltaTime);
               // other.transform.position = Vector3.SmoothDamp(other.transform.position, temp, ref velocity, dampModifier);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        foreach (string tag_ in pullableTags)
        {
            if (other.tag == tag_)
            {
               if(objectsToPull.Contains(other.gameObject))
                {
                    objectsToPull.Remove(other.gameObject);
                }
            }
        }
    }
    private void Update()
    {
        objectsToPull.RemoveAll(x => !x);
        foreach (GameObject obj in objectsToPull)
        {
          

            Vector3 temp = Vector3.Lerp(obj.transform.position, this.transform.position, pullSpeed * Time.deltaTime);
            obj.transform.position = Vector3.SmoothDamp(obj.transform.position, temp, ref velocity, dampModifier);
        }
       // objectsToPull.Clear();

    }
    
}
