using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomWallController : MonoBehaviour
{
    [SerializeField] List<GameObject> doors = new List<GameObject>();
    [SerializeField] List<GameObject> wallsNoDoors = new List<GameObject>();
    [SerializeField] List<GameObject> wallsWDoors = new List<GameObject>();
    [SerializeField] bool manualSetUp;
    [SerializeField] List<bool> manualActiveWallsWithDoors = new List<bool>();
    [SerializeField] List<bool> manualActiveWallsWithoutDoors = new List<bool>();
    private void Start()
    {
        SetUpRoom();
    }

    public void SetUpRoom()
    {
        ResetRoom();
        if(manualSetUp)
        {
            ToggleWallsWithDoors(manualActiveWallsWithDoors);
            ToggleWallsWithoutDoors(manualActiveWallsWithoutDoors);
        }

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
