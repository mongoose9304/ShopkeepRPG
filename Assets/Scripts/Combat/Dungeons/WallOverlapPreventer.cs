using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOverlapPreventer : MonoBehaviour
{
    //public bool disabledOtherObject;
    private void Start()
    {
        gameObject.transform.position += new Vector3(Random.Range(0.0001f, 0.00075f), Random.Range(0.0001f, 0.00075f), Random.Range(0.0001f, 0.00075f));
    }
    /*
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent( out WallOverlapPreventer wallOverlaper))
        {
            disabledOtherObject = true;
            wallOverlaper.DisableObject();
        }
    }
    public void DisableObject()
    {
        if (!disabledOtherObject)
            gameObject.transform.position +=new  Vector3(Random.Range(0.0001f, 0.00075f), Random.Range(0.0001f, 0.00075f), Random.Range(0.0001f, 0.00075f));
    }
    */
}
