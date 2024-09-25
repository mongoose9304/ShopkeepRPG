using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Room with traps that are meant to be dodged or navigated, more traps will be added in certain domains
/// </summary>
public class NavigationRoom : BasicRoom
{
    [SerializeField] List<GameObject> extraTraps;

    public override void StartRoomActivity()
    {
       
    }
    public override void ChangeSinType(SinType sin_)
    {
        if(sin_ == SinType.Lust)
        {
           // ActivateExtraTraps();
        }
    }
    private void ActivateExtraTraps()
    {
        if (extraTraps.Count == 0)
            return;
        foreach(GameObject obj in extraTraps)
        {
            obj.SetActive(true);
        }
    }
}
