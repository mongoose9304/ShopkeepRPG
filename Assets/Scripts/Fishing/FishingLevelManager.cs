using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLevelManager : MonoBehaviour
{

    public float prosperity;

    // Start is called before the first frame update
    void Start()
    {
        // Make sure prosperity is within range
        prosperity = Mathf.Clamp(prosperity, 0.5f, 1.5f);

        if (prosperity < 0.8f)
        {

        }
        else if (prosperity < 1.2f)
        {

        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
