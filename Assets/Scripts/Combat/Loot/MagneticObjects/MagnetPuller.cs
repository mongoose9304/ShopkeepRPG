using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPuller : MonoBehaviour
{
    public string[] pullableTags;
    public float pullSpeed;
    private Vector3 velocity = Vector3.zero;
    public float dampModifier;
    [SerializeField] private List<GameObject>objectsToPull =new List<GameObject>();
    private Rigidbody rb;
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

  
    private void FixedUpdate()
    {
        
        objectsToPull.RemoveAll(x => !x);
        foreach (GameObject obj in objectsToPull)
        {


            rb = obj.GetComponent<Rigidbody>();
           // rb.AddForce((transform.position - obj.transform.position).normalized * pullSpeed * Time.deltaTime, ForceMode.VelocityChange);
            rb.AddForce((transform.position - obj.transform.position).normalized * pullSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
            //rb.velocity= (transform.position - obj.transform.position).normalized* pullSpeed *Time.fixedDeltaTime;
        }
       // objectsToPull.Clear();

    }
    private void Update()
    {

        objectsToPull.RemoveAll(x => !x);
        foreach (GameObject obj in objectsToPull)
        {


            rb = obj.GetComponent<Rigidbody>();
            // rb.AddForce((transform.position - obj.transform.position).normalized * pullSpeed * Time.deltaTime, ForceMode.VelocityChange);
            rb.AddForce((transform.position - obj.transform.position).normalized * pullSpeed * Time.smoothDeltaTime, ForceMode.VelocityChange);
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
