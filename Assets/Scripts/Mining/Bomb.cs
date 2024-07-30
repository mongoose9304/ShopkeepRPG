using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float maxDetonationTime;
    float currentDetonationTime;
    public int range = 1;
    [SerializeField] LayerMask wallMask;
    [SerializeField] GameObject explosionEffect;

    public void Explode()
    {
        //right
        RaycastHit hit;
        Instantiate(explosionEffect, transform.position, transform.rotation);
        
        if (Physics.Raycast(transform.position, new Vector3(1, 0f, 0f), out hit, range*2 , wallMask))
        {
            Debug.Log(Mathf.RoundToInt(Vector3.Distance(transform.position, hit.transform.position)/2));
            int x = Mathf.RoundToInt(Vector3.Distance(transform.position, hit.transform.position) / 2);
            for (int i = 0; i <x-1; i++)
            {
                Instantiate(explosionEffect, transform.position + new Vector3(2 + (2.0f * i), 0.0f, 0f), transform.rotation);
            }
        }
        else
        {
            for(int i=0;i<range;i++)
            {
                Instantiate(explosionEffect, transform.position + new Vector3(2+(2.0f*i), 0.0f, 0f), transform.rotation);
            }
        }





        //left
        if (Physics.Raycast(transform.position, new Vector3(-1, 0f, 0f), out hit, range * 2, wallMask))
        {
            Debug.Log(Mathf.RoundToInt(Vector3.Distance(transform.position, hit.transform.position) / 2));
            int x = Mathf.RoundToInt(Vector3.Distance(transform.position, hit.transform.position) / 2);
            for (int i = 0; i < x - 1; i++)
            {
                Instantiate(explosionEffect, transform.position + new Vector3(-2 + (-2.0f * i), 0.0f, 0f), transform.rotation);
            }
        }
        else
        {
            for (int i = 0; i < range; i++)
            {
                Instantiate(explosionEffect, transform.position + new Vector3(-2 + (-2.0f * i), 0.0f, 0f), transform.rotation);
            }
        }
        //up
        if (Physics.Raycast(transform.position + new Vector3(0.5f, 0.0f, 0f), new Vector3(0, 0f, 1f), out hit, range * 2, wallMask))
        {
            Debug.Log(Mathf.RoundToInt(Vector3.Distance(transform.position, hit.transform.position) / 2));
            int x = Mathf.RoundToInt(Vector3.Distance(transform.position, hit.transform.position) / 2);
            for (int i = 0; i < x - 1; i++)
            {
                Instantiate(explosionEffect, transform.position + new Vector3(0, 0.0f, 2 + (2.0f * i)), transform.rotation);
            }
        }
        else
        {
            for (int i = 0; i < range; i++)
            {
                Instantiate(explosionEffect, transform.position + new Vector3(0, 0.0f, 2 + (2.0f * i)), transform.rotation);
            }
        }
        //down
        if (Physics.Raycast(transform.position + new Vector3(0.5f, 0.0f, 0f), new Vector3(0, 0f, -1f), out hit, range * 2, wallMask))
        {
            Debug.Log(Mathf.RoundToInt(Vector3.Distance(transform.position, hit.transform.position) / 2));
            int x = Mathf.RoundToInt(Vector3.Distance(transform.position, hit.transform.position) / 2);
            for (int i = 0; i < x - 1; i++)
            {
                Instantiate(explosionEffect, transform.position + new Vector3(0, 0.0f, -2 + (-2.0f * i)), transform.rotation);
            }
        }
        else
        {
            for (int i = 0; i < range; i++)
            {
                Instantiate(explosionEffect, transform.position + new Vector3(0, 0.0f, -2 + (-2.0f * i)), transform.rotation);
            }
        }
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        currentDetonationTime = maxDetonationTime;
    }
    private void Update()
    {
        currentDetonationTime -= Time.deltaTime;
        if (currentDetonationTime <= 0)
            Explode();
    }
}
