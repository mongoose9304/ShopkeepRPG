using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTeam : MonoBehaviour
{
    [SerializeField]
    private int team = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetTeam()
    {
        return team;
    }

    public void SetTeam(int _team)
    {
        team = _team;
    }
}
