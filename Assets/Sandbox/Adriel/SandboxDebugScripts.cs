using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandboxDebugScripts : MonoBehaviour
{
    public bool AddHungerCurse = false;
    public bool AddCowardiceCurse = false;
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
    }
}
