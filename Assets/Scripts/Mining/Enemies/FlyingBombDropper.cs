using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBombDropper : BasicMiningEnemy
{
    Vector3 moveDirection;
    Vector3 tileTragetPos;
    [SerializeField] float maxTimeMovingADirection;
    [SerializeField] float minTimeMovingADirection;
    float currentTimeMovingADirection;
    [SerializeField] LayerMask tileLayer;
    bool isGrounded=true;
    bool isMovingToTileCentre;
    private void Update()
    {
        if(isMovingToTileCentre)
        {
            Debug.Log("Moving to Tile");
            transform.position = Vector3.MoveTowards(transform.position,tileTragetPos, moveSpeed*Time.deltaTime*4);
            if(Vector3.Distance(transform.position,tileTragetPos)<1.0f)
            {
                isMovingToTileCentre = false;
                Debug.Log("I am done Moving to Tile");
            }
        }
        if(!isGrounded)
        {
            CenterOnTile();
            Rotate();
        }
         currentTimeMovingADirection -= Time.deltaTime;
         if (currentTimeMovingADirection <= 0)
             Rotate();
        Move();
    }
    private void Move()
    {
    
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
    private void Rotate()
    {
        CenterOnTile();
        currentTimeMovingADirection = Random.Range(minTimeMovingADirection, maxTimeMovingADirection);
        float x = Random.Range(0, 4);
       if(moveDirection==Vector3.right)
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
    }
    public override void DetectNoGround()
    {
        if (isMovingToTileCentre)
            return;
        isGrounded = false;
    }
    public override void DetectGround()
    {
        if (isMovingToTileCentre)
            return;
        isGrounded = true;
    }
    private void CenterOnTile()
    {
        Tile tile = GetCurrentTile();
        if(tile)
        {
            tileTragetPos = tile.transform.position + new Vector3(0, 4.5f, 0);
            isMovingToTileCentre = true;
        }

    }
    private Tile GetCurrentTile()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 10,tileLayer))
        {

            if (hit.collider.gameObject.TryGetComponent<Tile>(out Tile tile))
            {
                return tile;
            }
        }
        return null;
    }
}
