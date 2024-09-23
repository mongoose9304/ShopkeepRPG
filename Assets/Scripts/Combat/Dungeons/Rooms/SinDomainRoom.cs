using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinDomainRoom : BasicRoom
{
    public void StartConvo()
    {
        TempPause.instance.PauseForDialogue();
    }
    public void EndDialogue()
    {
        TempPause.instance.UnPauseForDialogue();
    }
}
