using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcedItemCollection : MonoBehaviour
{
    public string[] pullableTags;
    public float pullSpeed;
    private Vector3 velocity = Vector3.zero;
    public float dampModifier;
    [SerializeField] private List<GameObject> objectsToPull = new List<GameObject>();
    private void OnTriggerEnter(Collider other)
    {

        foreach (string tag_ in pullableTags)
        {
            if (other.tag == tag_)
            {
                objectsToPull.Add(other.gameObject);
                //  Vector3 temp = Vector3.Lerp(other.transform.position, this.transform.position, pullSpeed * Time.deltaTime);
                // other.transform.position = Vector3.SmoothDamp(other.transform.position, temp, ref velocity, dampModifier);
            }
        }
    }



    private void Update()
    {

        objectsToPull.RemoveAll(x => !x);
        foreach (GameObject obj in objectsToPull)
        {

            obj.transform.position = Vector3.MoveTowards(obj.transform.position, transform.position, pullSpeed * Time.deltaTime);
        }
          
        // objectsToPull.Clear();



        for (int i = 0; i < objectsToPull.Count; i++)
        {
            if (!objectsToPull[i].activeInHierarchy)
            {
                objectsToPull.RemoveAt(i);
            }
        }
    }
}
