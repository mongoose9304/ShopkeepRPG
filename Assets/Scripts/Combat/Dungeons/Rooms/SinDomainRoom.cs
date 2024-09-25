using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Room where players can talk to the ruler of this domain
/// </summary>
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
