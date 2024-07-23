using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] Vector3 fallDirection;
    [SerializeField] GameObject fallDirectionIndicator;
    [SerializeField] Transform[] fallDirectionIndicatorPositions;
    public int treeMaxHealth;
    [SerializeField] int treeCurrentHealth;
    bool isFalling;

    private void Start()
    {
        treeCurrentHealth = treeMaxHealth;
        isFalling = false;
    }
    //0=forward,1=right,2=backward;3=left
    public void HitTree(int direction_,int damage_=1)
    {
        if (isFalling)
            return;
        if (treeCurrentHealth <= 0)
        {
            Fall();
            return;
        }
        fallDirectionIndicator.gameObject.SetActive(true);
        switch (direction_)
        {
            case 0:
                fallDirection = new Vector3(0, 0, 1);
                fallDirectionIndicator.transform.position = fallDirectionIndicatorPositions[direction_].position;
                fallDirectionIndicator.transform.localRotation = fallDirectionIndicatorPositions[direction_].localRotation;
                break;
            case 1:
                fallDirection = new Vector3(1, 0, 0);
                fallDirectionIndicator.transform.position = fallDirectionIndicatorPositions[direction_].position;
                fallDirectionIndicator.transform.localRotation = fallDirectionIndicatorPositions[direction_].localRotation;
                break;
            case 2:
                fallDirection = new Vector3(0, 0, -1);
                fallDirectionIndicator.transform.position = fallDirectionIndicatorPositions[direction_].position;
                fallDirectionIndicator.transform.localRotation = fallDirectionIndicatorPositions[direction_].localRotation;
                break;
            case 3:
                fallDirection = new Vector3(-1, 0, 0);
                fallDirectionIndicator.transform.position = fallDirectionIndicatorPositions[direction_].position;
                fallDirectionIndicator.transform.localRotation = fallDirectionIndicatorPositions[direction_].localRotation;
                break;
        }
        treeCurrentHealth -= damage_;
        
        
    }
    public void Fall()
    {
        isFalling = true;
        fallDirectionIndicator.gameObject.SetActive(false);
    }

}
