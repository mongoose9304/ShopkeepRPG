using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingScript : MonoBehaviour
{
    public bool IsEquipped;
    public bool IsFishingReady;
    public bool IsCasted;
    public bool IsPullingLine;

    Animator animator;

    public GameObject Bait;
    public GameObject String;
    public GameObject FishingRod;
    

    



    
    void Start()
    {
        animator = GetComponent<Animator>();

        IsEquipped = true;



    }

   
    void Update()
    {
        
    }
}
