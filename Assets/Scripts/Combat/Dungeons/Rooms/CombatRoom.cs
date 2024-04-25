using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRoom : BasicRoom
{
    bool isLocked;



    public override void StartRoomActivity()
    {
        LockRoom(true);
    }
    private void LockRoom(bool lock_)
    {
        lockObject.SetActive(lock_);
    }
}
