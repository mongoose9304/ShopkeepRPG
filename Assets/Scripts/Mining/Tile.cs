using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
   [SerializeField]  bool IsOccupied;
   [SerializeField]protected Bomb myBomb;
   public bool typeBFloor;


    protected void Update()
    {
        if (myBomb)
        {
            if (!myBomb.gameObject.activeInHierarchy || Vector3.Distance(transform.position, myBomb.transform.position) > 1)
                myBomb = null;
        }
    }

    public void SetOccupied(bool isOccupied_)
    {
        IsOccupied = isOccupied_;
    }

    public void SetBomb(Bomb bomb_)
    {
        myBomb = bomb_;

    }
    public bool CanPlaceBomb()
    {
        if (!myBomb)
            return true;

        return false;
    }
}
