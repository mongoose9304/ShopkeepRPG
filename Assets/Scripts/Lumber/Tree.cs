using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public int treeHeight=1;
    public int woodMultiplier=1;
    [SerializeField] protected GameObject treeTrunkPrefab;
    [SerializeField] protected GameObject woodPrefab;
    [SerializeField] protected GameObject multiWoodPrefab;
    [SerializeField] protected GameObject treeTrunkHolder;
    [SerializeField] protected LineRenderer fallDirectionLineRenderer;
    [SerializeField] protected LayerMask wallMask;
    [SerializeField] protected Vector3 fallDirection=Vector3.zero;
    [SerializeField] protected Vector3 lineDirection=Vector3.zero;
    [SerializeField] protected Vector3 fallPivot;
    [SerializeField] protected float fallSpeed;
    [SerializeField] protected float fallSpeedIncrease;
    [SerializeField] protected float fallSpeedMax;
    [SerializeField] protected float fallSpeedMin;
    [SerializeField] protected GameObject fallDirectionIndicator;
    [SerializeField]protected Transform[] fallDirectionIndicatorPositions;
    [SerializeField]protected Transform[] fallDirectionPivotPositions;
    public List<GameObject> myTreeSections=new List<GameObject>();
    public int treeMaxHealth;
    [SerializeField]protected int treeCurrentHealth;
    public bool isFalling;
    protected bool hasBeenHit;
    private void Start()
    {
        treeCurrentHealth = treeMaxHealth;
        isFalling = false;
        hasBeenHit = false;
        for(int i=0;i<treeHeight;i++)
        {
           GameObject x= GameObject.Instantiate(treeTrunkPrefab, treeTrunkHolder.transform);
            x.transform.position += new Vector3(0, i+1, 0);
            myTreeSections.Add(x);
            x.GetComponent<TreeSection>().myTree = this;
        }
    }
    private void Update()
    {
        if(isFalling)
        {
            transform.RotateAround(fallPivot, -fallDirection, fallSpeed * Time.deltaTime);
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
                fallDirection = new Vector3(-1, 0, 0);
                lineDirection = new Vector3(0, 0, 1);
                break;
            case 1:
                fallDirection = new Vector3(0, 0, 1);
                lineDirection = new Vector3(1, 0, 0);
                break;
            case 2:
                fallDirection = new Vector3(1, 0, 0);
                lineDirection = new Vector3(0, 0, -1);
                break;
            case 3:
                fallDirection = new Vector3(0, 0, -1);
                lineDirection = new Vector3(-1, 0, 0);
                break;

        }
        fallDirectionIndicator.transform.position = fallDirectionIndicatorPositions[direction_].position;
        fallDirectionIndicator.transform.localRotation = fallDirectionIndicatorPositions[direction_].localRotation;
        fallPivot = fallDirectionPivotPositions[direction_].position;
        treeCurrentHealth -= damage_;
        RaycastHit hit;
       
        if (Physics.Raycast(transform.position, lineDirection, out hit ,treeHeight+2f, wallMask))
        {
            fallDirectionLineRenderer.SetPosition(0, transform.position);
            fallDirectionLineRenderer.SetPosition(1, hit.transform.position);
            fallDirectionLineRenderer.gameObject.SetActive(true);
        }
        else
        {
            fallDirectionLineRenderer.gameObject.SetActive(false);
        }
       




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
    protected virtual  void SpawnWood()
    {
        foreach(GameObject obj in myTreeSections)
        {
            GameObject.Instantiate(woodPrefab, obj.transform.position, obj.transform.rotation).GetComponent<LumberPickUp>().lumberAmount = woodMultiplier;
        }
        if(TreeManager.instance.GetCurrentCombo()==1)
        {
          GameObject.Instantiate(woodPrefab, transform.position, transform.rotation).GetComponent<LumberPickUp>().lumberAmount =woodMultiplier;
        }
        else if( TreeManager.instance.GetCurrentCombo() > 1)
        {
          GameObject x=  GameObject.Instantiate(multiWoodPrefab, transform.position, transform.rotation);
            x.GetComponent<LumberPickUp>().lumberAmount = TreeManager.instance.GetCurrentCombo()*woodMultiplier;
        }
    }
    public void BreakTree()
    {
        SpawnWood();
        gameObject.SetActive(false);
        GuardManager.instance.CreateNoise(transform, 2);
    }

}
