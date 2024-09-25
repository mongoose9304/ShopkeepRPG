using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///  Automatically opens and closes walls and doors based on if there is another room connected to this one. 
///  There are small colliders called connectors, as long as 2 are touching they will automatically open both rooms
/// </summary>
public class RoomWallController : MonoBehaviour
{
    [Tooltip("REFERENCE to the door objects ")]
    [SerializeField] List<GameObject> doors = new List<GameObject>();
    [Tooltip("REFERENCE connector objects ")]
    [SerializeField] List<GameObject> connectors = new List<GameObject>();
    [Tooltip("REFERENCE to solid walls without any holes")]
    [SerializeField] List<GameObject> wallsNoDoors = new List<GameObject>();
    [Tooltip("REFERENCE to the walls with holes for the doors ")]
    [SerializeField] List<GameObject> wallsWDoors = new List<GameObject>();
    [Tooltip("Stops the automatic enabling and disabling of walls")]
    [SerializeField] bool manualSetUp;
    [Tooltip("The layer to check for connectors")]
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
    /// <summary>
    /// Check if any nearby rooms connect to this one and if any do open the room up 
    /// </summary>
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

    /// <summary>
    ///  Lock this door 
    /// </summary>
    public void ToggleLocks(bool lock_)
    {
        foreach(GameObject obj in doors)
        {
            obj.SetActive(lock_);
        }
    }
}
