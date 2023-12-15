using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public enum TimeBlock{
        Morning,
        Noon,
        Evening,
        Night
    }

    [SerializeField] Vector3[] sunPositions = new Vector3[4];

    [SerializeField] Transform dirLight;
    [SerializeField] Light directionalLight;
    [SerializeField] TimeBlock currentTimeBlock = TimeBlock.Morning;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Shouldn't call this all the time, just here for debug purpose
        UpdateEnvironment(currentTimeBlock);
    }

    void UpdateEnvironment(TimeBlock timeBlock_){
        switch (timeBlock_){
            case TimeBlock.Morning:
                SetSunLightColor(new Color(0.8f, 0.8f, 0.6f));
                dirLight.rotation = Quaternion.Euler(sunPositions[0]);
                break;
            case TimeBlock.Noon:
                SetSunLightColor(new Color(1f, 1f, 1f));
                dirLight.rotation = Quaternion.Euler(sunPositions[1]);
                break;
            case TimeBlock.Evening:
                SetSunLightColor(new Color(0.6f, 0.4f, 0.2f));
                dirLight.rotation = Quaternion.Euler(sunPositions[2]);
                break;
            case TimeBlock.Night:
                SetSunLightColor(new Color(0.1f, 0.1f, 0.2f));
                dirLight.rotation = Quaternion.Euler(sunPositions[3]);
                break;
        }
    }

     void SetSunLightColor(Color colour)
    {
        if (directionalLight != null)
        {
            directionalLight.color = colour;
        }
    }
}
