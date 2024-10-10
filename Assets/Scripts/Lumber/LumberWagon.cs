using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberWagon : MonoBehaviour
{
   public void ActivateWagon()
    {
        LumberLevelManager.instance.WinLevel();
    }
}
