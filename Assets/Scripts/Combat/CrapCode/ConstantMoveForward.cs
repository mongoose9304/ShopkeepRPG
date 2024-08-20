using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantMoveForward : MonoBehaviour
{
    [SerializeField] float MaxLifeTime;
    [SerializeField] float speed;
    float currentLifeTime;
    private void OnEnable()
    {
        currentLifeTime = MaxLifeTime;
    }
    private void Update()
    {
        currentLifeTime -= Time.deltaTime;
        if(currentLifeTime<=0)
        {
            gameObject.SetActive(false);
        }
        transform.position += transform.forward * Time.deltaTime * speed;
    }
}
