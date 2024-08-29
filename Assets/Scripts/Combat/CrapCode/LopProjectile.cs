using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LopProjectile : MonoBehaviour
{
    public Vector3 target;
    [SerializeField] float MaxLifeTime;
    [SerializeField] float speed;
    [SerializeField] float startDescentDistance;
    float currentLifeTime;
    Quaternion rotation;
    Vector3 direction;
    private void OnEnable()
    {
        currentLifeTime = MaxLifeTime;
    }
    private void Update()
    {
        direction = target - transform.position;
        direction.y = 0; // keep the direction strictly horizontal
        rotation = Quaternion.LookRotation(direction);
        // slerp to the desired rotation over time
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 99 * Time.deltaTime);
        currentLifeTime -= Time.deltaTime;
        if (currentLifeTime <= 0)
        {
            gameObject.SetActive(false);
        }
        transform.position += transform.forward * Time.deltaTime * speed;
        
          if (startDescentDistance <= Vector2.Distance(new Vector2(transform.position.x,transform.position.z), new Vector2(target.x, target.z)))
          {

              transform.position = new Vector3(transform.position.x, transform.position.y+Time.deltaTime* 4, transform.position.z);
          }
          else
          {
              transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * 9.8f, transform.position.z);
          }
        if (transform.position.y > 5)
        {
            transform.position = new Vector3(transform.position.x, 5, transform.position.z);
        }

    }
}
