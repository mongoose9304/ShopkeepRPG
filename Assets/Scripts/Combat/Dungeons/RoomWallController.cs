using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///  Automatically opens and closes walls and doors based on if there is another room connected to this one. 
///  There are small colliders called connectors, as long as 2 are touching they will automatically open both rooms
/// </summary>
public class RoomWallController : MonoBehaviour
{
    [SerializeField] List<GameObject> connectors = new List<GameObject>();
    [Tooltip("REFERENCE to solid walls without any holes")]
    [SerializeField] List<GameObject> walls = new List<GameObject>();
    [Tooltip("The layer to check for connectors")]
    [SerializeField] LayerMask connectorLayer;
    private void OnEnable()
    {
        SetUpRoom();
    }

    public void SetUpRoom()
    {

        StartCoroutine(WaitAFrame());
       

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

             List<bool> x= new List<bool>();
        foreach(GameObject obj in connectors)
        {
            Collider[] hitColliders = Physics.OverlapSphere(obj.transform.position, 4.0F,connectorLayer);
            if(hitColliders.Length <= 1)
            {
                x.Add(true);
                
                continue;
            }
            foreach (var hitCollider in hitColliders)
            {
                if (connectors.Contains(hitCollider.gameObject))
                {
                }
                else
                {
                    x.Add(false);
                    break;
                }
            }
           
           
        }
        
            ToggleWalls(x);

    }

    //0= east, 1=north,2-west,3=south
    public void ToggleWalls(List<bool> ActiveWalls_)
    {
        for(int i=0;i<walls.Count;i++)
        {

            walls[i].SetActive(ActiveWalls_[i]);
            connectors[i].SetActive(ActiveWalls_[i]);

        }
    }

}
