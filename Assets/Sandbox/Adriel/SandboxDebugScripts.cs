using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandboxDebugScripts : MonoBehaviour
{
    public bool AddHungerCurse = false;
    public bool AddCowardiceCurse = false;
    public bool QuitEarly = false;
    public CombatPlayerMovement cpm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (AddHungerCurse) {
            AddHungerCurse = false;
            CombatExtrenalModManager.instance.AddModToAllPlayers("Hunger");
        }

        if (AddCowardiceCurse) {
            AddCowardiceCurse = false;
            CombatExtrenalModManager.instance.AddModToAllPlayers("Cowardice");
        }

        if (QuitEarly) {
            QuitEarly = false;
            cpm.TrueDeath();
        }
    }
}
