using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcedItemCollection : MonoBehaviour
{
    public string[] pullableTags;
    public float pullSpeed;
    public float lookSpeed;
    private Vector3 velocity = Vector3.zero;
    private Vector3 direction = Vector3.zero;
    private Quaternion rotation;
    public float dampModifier;
    [SerializeField] private List<GameObject> objectsToPull = new List<GameObject>();
    private void OnTriggerEnter(Collider other)
    {

        foreach (string tag_ in pullableTags)
        {
            if (other.tag == tag_)
            {
                if (!objectsToPull.Contains(other.gameObject))
                {
                    objectsToPull.Add(other.gameObject);
                    if(other.gameObject.TryGetComponent(out Rigidbody rb))
                    {
                        rb.isKinematic = true;
                    }

                }

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
            direction = transform.position - obj.transform.position;
            rotation = Quaternion.LookRotation(direction);
            obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, rotation, lookSpeed * Time.deltaTime);
            obj.transform.position += (obj.transform.forward * Time.deltaTime * pullSpeed);
            //obj.transform.position = Vector3.MoveTowards(obj.transform.position, transform.position, pullSpeed * Time.deltaTime);
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
