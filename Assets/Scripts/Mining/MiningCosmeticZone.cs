using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningCosmeticZone : MonoBehaviour
{
    public GameObject maxDecorations;
    public GameObject highDecorations;
    public GameObject lowDecorations;
    public GameObject minDecorations;
    public void SetUpCosmetics(float health_)
    {
        
            minDecorations.SetActive(false);
            maxDecorations.SetActive(false);
            lowDecorations.SetActive(false);
            highDecorations.SetActive(false);
            if (health_ < 0.6f)
            {
                //Min
                minDecorations.SetActive(true);
            }
            else if (health_ < 0.8f)
            {
                //Low
                lowDecorations.SetActive(true);
            }
            else if (health_ < 1.2f)
            {
                //Medium
                lowDecorations.SetActive(true);
                highDecorations.SetActive(true);
            }
            else if (health_ < 1.4)
            {
                //High
                lowDecorations.SetActive(true);
                highDecorations.SetActive(true);
            }
            else
            {
                lowDecorations.SetActive(true);
                highDecorations.SetActive(true);
                maxDecorations.SetActive(true);
                //Max
            }
        }
    
}
