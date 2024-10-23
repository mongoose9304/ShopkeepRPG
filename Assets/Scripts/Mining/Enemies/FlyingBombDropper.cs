using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBombDropper : BasicMiningEnemy
{
    Vector3 moveDirection;
   [SerializeField] protected Vector3 tileTargetPos;
    protected Vector3 landingTargetPos;
    [SerializeField] protected int maxDistance;
    [SerializeField] protected int minDistance;
    [SerializeField] protected int tileSize=2;
    [SerializeField] protected float maxLandingTime;
    [SerializeField]protected float maxFlyingTime;
    [SerializeField] protected float landingDistance;
    [SerializeField] GameObject bombPrefab;
    [SerializeField]public GameObject visualBomb;
    public float flyingTime;
   public float landingTime;
   public float attackTime;
    [SerializeField]protected LayerMask tileLayer;
    protected bool isLanding =true;
    GameObject myBomb;

    protected override void Start()
    {
        base.Start();
        flyingTime = maxFlyingTime;
        tileTargetPos = transform.position;
        landingTime = maxLandingTime;
        myBomb = Instantiate(bombPrefab);
        myBomb.SetActive(false);
        attackTime = maxAttackCooldown;
        isLanding = false;
    }
    private void Update()
    {
        if(isLanding)
        {
            if (landingTime > 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, landingTargetPos, moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, landingTargetPos+new Vector3(0,landingDistance,0), moveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, landingTargetPos + new Vector3(0, landingDistance, 0)) < 0.1f)
                {
                    isLanding = false;
                    flyingTime = maxFlyingTime;
                }
            }
            landingTime -= Time.deltaTime;
            return;
        }
        Move();
    }
    private void Move()
    {
        flyingTime -= Time.deltaTime;
        attackTime -= Time.deltaTime;
        if(attackTime<1)
        {
            visualBomb.SetActive(true);
        }
       transform.position= Vector3.MoveTowards(transform.position, tileTargetPos, moveSpeed*Time.deltaTime);
        if(Vector3.Distance(transform.position, tileTargetPos) <0.1f)
        {
            Rotate();
        }
    }
    private void Rotate()
    {
        float i = Random.Range(minDistance, maxDistance)*2;
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
        tileTargetPos = transform.position + (moveDirection * i);
        if (!Physics.Raycast(tileTargetPos, Vector3.down, 10, tileLayer))
        {
            Rotate();
        }
        else
        {
            if(flyingTime<=0)
            {
                Land();
            }
            else if(attackTime<=0)
            {
                DropBomb();
            }
        }
    }
    private void Land()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 10,tileLayer))
        {

            if (hit.collider.gameObject.TryGetComponent<Tile>(out Tile tile))
            {
                isLanding = true;
                landingTargetPos = transform.position -new  Vector3(0, landingDistance, 0);
                landingTime = maxLandingTime;
            }
        }
    }
    protected virtual void DropBomb()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 10, tileLayer))
        {

            if (hit.collider.gameObject.TryGetComponent<Tile>(out Tile tile))
            {
                visualBomb.SetActive(false);
                myBomb.transform.position = visualBomb.transform.position;
                myBomb.transform.rotation = visualBomb.transform.rotation;
                myBomb.GetComponent<MoveTowardsTarget>().target = transform.position - new Vector3(0, landingDistance, 0);
                myBomb.SetActive(true);
                attackTime = maxAttackCooldown;
            }
        }
       
    }
   

}
