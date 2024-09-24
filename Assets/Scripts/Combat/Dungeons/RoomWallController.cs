using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///  Automatically opens and closes walls and doors based on if there is another room connected to this one. 
///  There are small colliders called connectors, as long as 2 are touching they will automatically open both rooms
/// </summary>
public class RoomWallController : MonoBehaviour
{
    [SerializeField] List<GameObject> doors = new List<GameObject>();
    [SerializeField] List<GameObject> connectors = new List<GameObject>();
    [SerializeField] List<GameObject> wallsNoDoors = new List<GameObject>();
    [SerializeField] List<GameObject> wallsWDoors = new List<GameObject>();
    [SerializeField] bool manualSetUp;
    [SerializeField] LayerMask connectorLayer;
    private void OnEnable()
    {
        SetUpRoom();
    }

    public void SetUpRoom()
    {
        ResetRoom();
        if(manualSetUp)
        {

        }
        else
        {
            StartCoroutine(WaitAFrame());
        }

    }
    IEnumerator WaitAFrame()
    {
        yield return new WaitForSeconds(0.1f);
        CheckConnections();
    }
    private void CheckConnections()
    {
        if (manualSetUp)
            return;

             List<bool> x= new List<bool>();
        foreach(GameObject obj in connectors)
        {
            Collider[] hitColliders = Physics.OverlapSphere(obj.transform.position, 2.0F,connectorLayer);
            if(hitColliders.Length == 1)
            {
                x.Add(false);
                
                continue;
            }
            foreach (var hitCollider in hitColliders)
            {
                if (connectors.Contains(hitCollider.gameObject))
                {
                }
                else
                {
                    x.Add(true);
                    break;
                }
            }
           
           
        }
        
            ToggleWallsWithDoors(x);
       for(int i=0;i<x.Count; i++)
        {
            x[i] = !x[i];
        }
        ToggleWallsWithoutDoors(x);

    }
    private void ResetRoom()
    {

        foreach (GameObject obj in doors)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in wallsNoDoors)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in wallsWDoors)
        {
            obj.SetActive(false);
        }
    }
    //0= east, 1=north,2-west,3=south
    public void ToggleWallsWithDoors(List<bool> ActiveWalls_)
    {
        for(int i=0;i<wallsWDoors.Count;i++)
        {
            
          wallsWDoors[i].SetActive(ActiveWalls_[i]);
            
        }
    }
    public void ToggleWallsWithoutDoors(List<bool> ActiveWalls_)
    {
        for (int i = 0; i < wallsNoDoors.Count; i++)
        {

            wallsNoDoors[i].SetActive(ActiveWalls_[i]);

        }
    }


    public void ToggleLocks(bool lock_)
    {
        foreach(GameObject obj in doors)
        {
            obj.SetActive(lock_);
        }
    }
}
