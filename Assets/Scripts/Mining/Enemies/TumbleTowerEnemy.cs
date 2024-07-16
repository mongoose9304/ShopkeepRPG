using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbleTowerEnemy : BasicMiningEnemy
{
    Vector3 tileTargetPos;
    Vector3 moveDirection;
    [SerializeField] float minDistance;
    [SerializeField] float maxDistance;
    [SerializeField] LayerMask tileLayer;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] Vector3[] towerPositions;
    List<GameObject> towerPieces=new List<GameObject>();
    [SerializeField] List<GameObject> towerPiecesRef=new List<GameObject>();
    protected override void Start()
    {
        base.Start();
        tileTargetPos = transform.position;
        towerPieces = towerPiecesRef;
    }
    private void Update()
    {
        Move();
        RearangeTower();
        MoveTowerParts();
    }
    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, tileTargetPos, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, tileTargetPos) < 0.1f)
        {
            Rotate();
        }

    }
    private void MoveTowerParts()
    {
        for(int i=0;i<towerPieces.Count;i++)
        {
            towerPieces[i].transform.localPosition = Vector3.MoveTowards(towerPieces[i].transform.localPosition, towerPositions[i],moveSpeed*2*Time.deltaTime);
        }
    }
    private void RearangeTower()
    {
        for (int i = 0; i < towerPieces.Count; i++)
        {
            if (towerPieces[i].activeInHierarchy == false)
                towerPieces.Remove(towerPieces[i]);
        }
    }
    private void Rotate()
    {
        float i = Random.Range(minDistance, maxDistance) * 2;
        float x = Random.Range(0, 4);
        if (moveDirection == Vector3.right)
        {
            if (x == 1)
                x = 0;
        }
        else if (moveDirection == Vector3.left)
        {
            if (x == 2)
                x = 3;
        }
        else if (moveDirection == Vector3.forward)
        {
            if (x == 3)
                x = 1;
        }
        else if (moveDirection == Vector3.back)
        {
            if (x == 0)
                x = 2;
        }
        switch (x)
        {
            case 1:
                moveDirection = Vector3.right;
                break;
            case 2:
                moveDirection = Vector3.left;
                break;
            case 3:
                moveDirection = Vector3.forward;
                break;
            case 0:
                moveDirection = Vector3.back;
                break;
        }
        tileTargetPos = transform.position + (moveDirection * i);
        if (!Physics.Raycast(tileTargetPos+new Vector3(0,4,0), Vector3.down, 10, tileLayer))
        {
            Rotate();
            return;
        }
        if (Physics.Raycast(tileTargetPos + new Vector3(0, 4, 0), Vector3.down, 10, obstacleLayer))
        {
            Rotate();
            return;
        }
    }
}
