using Shopkeeper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemVaccuum : MonoBehaviour
{
    private List<GameObject> items = new List<GameObject>();

    [SerializeField]
    AnimationCurve curve;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            items.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            items.Remove(other.gameObject);
        }
    }

    void FixedUpdate()
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (items[i]) 
            {
                float distance = Vector3.Distance(items[i].transform.position, transform.position);

                float moveSpeed = curve.Evaluate(distance);
                 
                items[i].transform.position = Vector3.MoveTowards(items[i].transform.position, transform.position, moveSpeed * Time.deltaTime);
            }
            else
            {
                items.RemoveAt(i);
            }
        }
    }
}
