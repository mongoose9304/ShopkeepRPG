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
        if (!Physics.Raycast(transform.position + new Vector3(0.5f, 0.0f, 0f), new Vector3(1, 0f, 0f), range * 2, wallMask))
        {
            Instantiate(explosionEffect, transform.position + new Vector3(2.0f, 0.0f, 0f), transform.rotation);
        }
        //left
        if (!Physics.Raycast(transform.position + new Vector3(0.5f, 0.0f, 0f), new Vector3(-1, 0f, 0f), range * 2, wallMask))
        {
            Instantiate(explosionEffect, transform.position + new Vector3(-2.0f, 0.0f, 0f), transform.rotation);
        }
        //up
        if (!Physics.Raycast(transform.position + new Vector3(0.5f, 0.0f, 0f), new Vector3(0, 0f, 1f), range * 2, wallMask))
        {
            Instantiate(explosionEffect, transform.position + new Vector3(0.0f, 0.0f, 2.0f), transform.rotation);
        }
        //down
        if (!Physics.Raycast(transform.position + new Vector3(0.5f, 0.0f, 0f), new Vector3(0, 0f, -1f), range * 2, wallMask))
        {
            Instantiate(explosionEffect, transform.position + new Vector3(0.0f, 0.0f, -2.0f), transform.rotation);
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
