using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.Tools;

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
    [SerializeField] protected GameObject ChopInteraction;
    [SerializeField] protected GameObject FallInteraction;
    [SerializeField] protected UnityEvent treeBrokenEvent;
    public List<GameObject> myTreeSections=new List<GameObject>();
    public int treeMaxHealth;
    [SerializeField]protected int treeCurrentHealth;
    public bool isFalling;
    protected bool hasBeenHit;
    [SerializeField] protected MMProgressBar myHealthBar;
    public AudioClip[] chopAudios;
    public AudioSource fallingAudio;
    public AudioClip fallingCrashIntoOtherTree;
    protected virtual void Start()
    {
        treeCurrentHealth = treeMaxHealth;
        UpdateHealthBar();
        myHealthBar.gameObject.SetActive(false);
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
        MMSoundManager.Instance.PlaySound(chopAudios[Random.Range(0,chopAudios.Length)], MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
        false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
        1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
        hasBeenHit = true;
        fallDirectionIndicator.gameObject.SetActive(true);
        switch (direction_)
        {
            case 0:
                fallDirection = new Vector3(1, 0, 0);
                lineDirection = new Vector3(0, 0, -1);
                break;
            case 1:
                fallDirection = new Vector3(0, 0, -1);
                lineDirection = new Vector3(-1, 0, 0);
                break;
            case 2:
                fallDirection = new Vector3(-1,0, 0);
                lineDirection = new Vector3(0, 0, 1);
                break;
            case 3:
                fallDirection = new Vector3(0, 0, 1);
                lineDirection = new Vector3(1, 0, 0);
                break;

        }
        fallDirectionIndicator.transform.position = fallDirectionIndicatorPositions[direction_].position;
        fallDirectionIndicator.transform.localRotation = fallDirectionIndicatorPositions[direction_].localRotation;
        fallPivot = fallDirectionPivotPositions[direction_].position;
        treeCurrentHealth -= damage_;
        UpdateHealthBar();
        myHealthBar.gameObject.SetActive(true);
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
        if (treeCurrentHealth <= 0)
        {
            // ChopInteraction.SetActive(false);
            FallInteraction.SetActive(true);
        }




    }
    public void Fall()
    {
        if (isFalling)
            return;
        fallingAudio.Play();
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
            LumberLevelManager.instance.SpawnLumber(obj.transform, woodMultiplier);
        }
        if(TreeManager.instance.GetCurrentCombo()==1)
        {
            LumberLevelManager.instance.SpawnLumber(transform, woodMultiplier);
        }
        else if( TreeManager.instance.GetCurrentCombo() > 1)
        {
            LumberLevelManager.instance.SpawnLumber(transform, Mathf.RoundToInt(woodMultiplier*TreeManager.instance.GetCurrentCombo()*LumberLevelManager.instance.forestHealth),true);
        }
    }
    public void BreakTree()
    {
        SpawnWood();
        gameObject.SetActive(false);
        GuardManager.instance.CreateNoise(transform, 2);
        treeBrokenEvent.Invoke();
        LumberLevelManager.instance.TreeFall();
    }
    public void HitOtherTreeAudio()
    {
        fallingAudio.Stop();
        MMSoundManager.Instance.PlaySound(fallingCrashIntoOtherTree, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
       false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
       1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);

    }
    private void UpdateHealthBar()
    {
        myHealthBar.UpdateBar(treeCurrentHealth ,0, treeMaxHealth);
    }
    public void RemoveStump()
    {
        LumberLevelManager.instance.SpawnLumber(transform, woodMultiplier);
    }
    public void ReplantTree()
    {
        LumberLevelManager.instance.TreeReplanted();
    }

}
