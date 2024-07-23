using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] Vector3 fallDirection;
    [SerializeField] Vector3 fallPivot;
    [SerializeField] float fallSpeed;
    [SerializeField] float fallSpeedIncrease;
    [SerializeField] float fallSpeedMax;
    [SerializeField] float fallSpeedMin;
    [SerializeField] GameObject fallDirectionIndicator;
    [SerializeField] Transform[] fallDirectionIndicatorPositions;
    [SerializeField] Transform[] fallDirectionPivotPositions;
    public List<GameObject> myTreeSections=new List<GameObject>();
    public int treeMaxHealth;
    [SerializeField] int treeCurrentHealth;
    public bool isFalling;
    bool hasBeenHit;
    private void Start()
    {
        treeCurrentHealth = treeMaxHealth;
        isFalling = false;
        hasBeenHit = false;
    }
    private void Update()
    {
        if(isFalling)
        {
            transform.RotateAround(fallPivot, fallDirection, fallSpeed * Time.deltaTime);
            //transform.Rotate(fallDirection,fallSpeed*Time.deltaTime,Space.Self);
            fallSpeed += fallSpeedIncrease * Time.deltaTime;
            if (fallSpeed >= fallSpeedMax)
                fallSpeed = fallSpeedMax;
        }
    }
    //0=forward,1=right,2=backward;3=left
    public void HitTree(int direction_,int damage_=1)
    {
        if (isFalling)
            return;
        hasBeenHit = true;
        if (treeCurrentHealth <= 0)
        {
            Fall();
            return;
        }
        fallDirectionIndicator.gameObject.SetActive(true);
        switch (direction_)
        {
            case 0:
                fallDirection = new Vector3(1, 0, 0);
                fallDirectionIndicator.transform.position = fallDirectionIndicatorPositions[direction_].position;
                fallDirectionIndicator.transform.localRotation = fallDirectionIndicatorPositions[direction_].localRotation;
                fallPivot = fallDirectionPivotPositions[direction_].position;
                break;
            case 1:
                fallDirection = new Vector3(0, 0, -1);
                fallDirectionIndicator.transform.position = fallDirectionIndicatorPositions[direction_].position;
                fallDirectionIndicator.transform.localRotation = fallDirectionIndicatorPositions[direction_].localRotation;
                fallPivot = fallDirectionPivotPositions[direction_].position;
                break;
            case 2:
                fallDirection = new Vector3(-1, 0, 0);
                fallDirectionIndicator.transform.position = fallDirectionIndicatorPositions[direction_].position;
                fallDirectionIndicator.transform.localRotation = fallDirectionIndicatorPositions[direction_].localRotation;
                fallPivot = fallDirectionPivotPositions[direction_].position;
                break;
            case 3:
                fallDirection = new Vector3(0, 0, 1);
                fallDirectionIndicator.transform.position = fallDirectionIndicatorPositions[direction_].position;
                fallDirectionIndicator.transform.localRotation = fallDirectionIndicatorPositions[direction_].localRotation;
                fallPivot = fallDirectionPivotPositions[direction_].position;
                break;
        }
        treeCurrentHealth -= damage_;
        
        
    }
    public void Fall()
    {
        if (isFalling)
            return;
        isFalling = true;
        fallSpeed = fallSpeedMin;
        fallDirectionIndicator.gameObject.SetActive(false);
        if(!hasBeenHit)
        {
            int x = Random.Range(0, 4);
            switch (x)
            {
                case 0:
                    fallDirection = new Vector3(1, 0, 0);
                    fallDirectionIndicator.transform.position = fallDirectionIndicatorPositions[x].position;
                    fallDirectionIndicator.transform.localRotation = fallDirectionIndicatorPositions[x].localRotation;
                    fallPivot = fallDirectionPivotPositions[x].position;
                    break;
                case 1:
                    fallDirection = new Vector3(0, 0, -1);
                    fallDirectionIndicator.transform.position = fallDirectionIndicatorPositions[x].position;
                    fallDirectionIndicator.transform.localRotation = fallDirectionIndicatorPositions[x].localRotation;
                    fallPivot = fallDirectionPivotPositions[x].position;
                    break;
                case 2:
                    fallDirection = new Vector3(-1, 0, 0);
                    fallDirectionIndicator.transform.position = fallDirectionIndicatorPositions[x].position;
                    fallDirectionIndicator.transform.localRotation = fallDirectionIndicatorPositions[x].localRotation;
                    fallPivot = fallDirectionPivotPositions[x].position;
                    break;
                case 3:
                    fallDirection = new Vector3(0, 0, 1);
                    fallDirectionIndicator.transform.position = fallDirectionIndicatorPositions[x].position;
                    fallDirectionIndicator.transform.localRotation = fallDirectionIndicatorPositions[x].localRotation;
                    fallPivot = fallDirectionPivotPositions[x].position;
                    break;
            }
        }
    }
    public void BreakTree()
    {
        gameObject.SetActive(false);
    }

}
