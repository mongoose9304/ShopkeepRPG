using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSlick : MonoBehaviour
{
    // How low the prosperity needs to be for this to appear
    public float prosperityMin = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        FishingLevelManager levelManager = GameObject.Find("FishingSceneManager").GetComponent<FishingLevelManager>();
        if (prosperityMin <= levelManager.prosperity)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
