using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbleTowerOverlord : MonoBehaviour
{
    [SerializeField] Vector3[] towerPositions;
    Vector3 targetPos;
    List<GameObject> towerPieces = new List<GameObject>();
    [SerializeField] List<GameObject> towerPiecesRef = new List<GameObject>();
    [SerializeField] float towerArrangeSpeed;
    [SerializeField] float moveSpeed;
    [SerializeField] float slamSpeed;
    [SerializeField] float slamSpeedModifier;
    [SerializeField] float slamSpeedModifierMax;
    [SerializeField] float slamSpeedModifierMaxIncrease;
    [SerializeField] int maxX;
    [SerializeField] int minX;
    [SerializeField] bool isMoving;
    [SerializeField] bool isSlaming;
    private void Start()
    {
        towerPieces = towerPiecesRef;
        GetRandomTarget();
        isMoving = true;
    }
    private void Update()
    {
       
        RearangeTower();
        MoveTowerParts();
        SpecialSlamAttack();
    }
    private void MoveTowerParts()
    {

        for (int i = 0; i < towerPieces.Count; i++)
        {
            towerPieces[i].transform.localPosition = Vector3.MoveTowards(towerPieces[i].transform.localPosition, towerPositions[i], towerArrangeSpeed * Time.deltaTime);
            if (Vector3.Distance(towerPieces[0].transform.localPosition, towerPositions[0]) < 0.1f)
                towerPieces[0].GetComponent<TumbleSection>().ChangeColliders(true);
            else
                towerPieces[0].GetComponent<TumbleSection>().ChangeColliders(false);
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
    private void GetRandomTarget()
    {
        targetPos = transform.localPosition;
        targetPos.x = Random.Range(minX, maxX);
    }
    private void Move()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.localPosition, targetPos) < 0.1f)
        {
            isMoving = false;
        }
    }                                                                                                   
    private void SpecialSlamAttack()
    {
        Debug.Log(transform.localEulerAngles.x);
        if(!isMoving)
        {
            if (!isSlaming)
            {
                Debug.Log(transform.localRotation.x);
                transform.Rotate(new Vector3(-1,0,0)*Time.deltaTime * slamSpeed * slamSpeedModifier, Space.Self);
                slamSpeedModifier += slamSpeedModifierMaxIncrease * Time.deltaTime;
                if (slamSpeedModifier > slamSpeedModifierMax)
                    slamSpeedModifier = slamSpeedModifierMax;
                if (transform.rotation.x>=90)
                {
                    slamSpeedModifier = 1;
                    isSlaming = true;
                }
              
              
            }
            else
            {
                transform.eulerAngles += new Vector3(1, 0, 0) * Time.deltaTime * slamSpeed*4;
                if (transform.localEulerAngles.x <= 2&& transform.localEulerAngles.x>=-2)
                {
                    isSlaming = false;
                    isMoving = true;
                    GetRandomTarget();
                }
            }
        }
        else
        {
            Move();
        }
    }

}
