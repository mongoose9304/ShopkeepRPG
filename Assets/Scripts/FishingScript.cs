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

    public GameObject TheBaitPrefab;
    public GameObject End_Of_Rope;
    public GameObject Start_Of_Rope;
    public GameObject Start_Of_Rod;

    Transform baitPosition;



    
    void Start()
    {
        animator = GetComponent<Animator>();

        IsEquipped = true;



    }

   
    void Update()
    {
        
    }
}
