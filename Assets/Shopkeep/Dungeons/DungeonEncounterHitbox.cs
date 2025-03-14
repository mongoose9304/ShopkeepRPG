using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEncounterHitbox : MonoBehaviour
{
    [SerializeField]
    public List<BoxCollider> colliders = new List<BoxCollider>();
    private DungeonEncounter encounter;

    private int playersEntered;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
    }
}
