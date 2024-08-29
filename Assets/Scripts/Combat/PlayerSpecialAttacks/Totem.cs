using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour
{
    public GameObject eyeball;
    [SerializeField] bool lookAtEnemies;
    [SerializeField] List<GameObject> currentFoes;
    [SerializeField] GameObject currentTarget;
    [SerializeField] GameObject damageObject;
    public PlayerDamageCollider damgeCollider;

    private void Update()
    {
        if(!currentTarget)
            FindNewTarget();
        else if (!currentTarget.activeInHierarchy)
            FindNewTarget();

        FaceTarget();
    }

    private void FaceTarget()
    {
        if(lookAtEnemies)
        {
            if(currentTarget)
            {
                eyeball.transform.LookAt(currentTarget.transform,Vector3.up);
                damageObject.SetActive(true);
            }
            else
            {
                damageObject.SetActive(false);
            }
        }
    }
    private void FindNewTarget()
    {
        if (currentFoes.Count == 0)
            return;

        for(int i=0;i<currentFoes.Count;i++)
        {
            if(!currentFoes[i].activeInHierarchy)
            {
                currentFoes.RemoveAt(i);
            }
        }
        if (currentFoes.Count == 0)
        {
            currentTarget = null;
            return;
        }
        currentTarget = currentFoes[0];

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Enemy")
        {
            if(!currentFoes.Contains(other.gameObject))
            {
                currentFoes.Add(other.gameObject);
            }
        }
    }
    private void OnEnable()
    {
        currentFoes.Clear();
        eyeball.transform.rotation = new Quaternion(0, 0, 0, 0);
    }
    
}
