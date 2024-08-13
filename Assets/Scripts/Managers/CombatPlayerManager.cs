using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPlayerManager : MonoBehaviour
{
    public static CombatPlayerManager instance;
    [SerializeField] CombatPlayerActions[] players;
    private void Awake()
    {
        instance = this;
    }

    public void ReturnFamiliars()
    {
        foreach(CombatPlayerActions player_ in players)
        {
            player_.ReturnFamiliar();
        }
    }
}
