using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DungeonEncounterPhase {
    public UnityEvent start;
    public UnityEvent end;

    DungeonEncounterPhase(UnityEvent ev)
    {
        ev.AddListener(OnPhaseStart);
        ev.AddListener(OnPhaseEnd);
    }

    void OnPhaseStart()
    {

    }

    void OnPhaseEnd()
    {

    }
}


public class DungeonEncounter : MonoBehaviour
{
    public List<DungeonEncounterPhase> phases;
    public List<DungeonEncounterDoor> dungeonDoors;

    private int phaseCounter;

    UnityEvent phaseStart;
    UnityEvent phaseEnd;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {

    }

    public void DecrementPhase()
    {
        phaseCounter--;
        if (phaseCounter <= 0)
        {
            phases[0].end.Invoke();
            phases.RemoveAt(0);
            if (phases.Count > 0)
            {
                // Activate the next phase
            }
        }
    }
}
