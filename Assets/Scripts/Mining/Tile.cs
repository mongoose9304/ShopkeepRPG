using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The tiles or squares that make up a level
/// </summary>
public class Tile : MonoBehaviour
{
   [Tooltip("The bomb that has been placed on this tile")]
   [SerializeField]protected Bomb myBomb;
   [Tooltip("Type B floors will use the B material in a level, used to create checkerboard designs")]
   public bool typeBFloor;


    protected void Update()
    {
        if (myBomb)
        {
            if (!myBomb.gameObject.activeInHierarchy || Vector3.Distance(transform.position, myBomb.transform.position) > 2)
                myBomb = null;
        }
    }


    /// <summary>
    /// Set the reference to the bomb placed on this tile
    /// </summary>
    public void SetBomb(Bomb bomb_)
    {
        myBomb = bomb_;

    }
    /// <summary>
    /// Check if there is already a bomb placed here or not
    /// </summary>
    public bool CanPlaceBomb()
    {
        if (!myBomb)
            return true;

        return false;
    }
}
