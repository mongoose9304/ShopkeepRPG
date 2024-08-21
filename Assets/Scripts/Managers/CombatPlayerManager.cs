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
    public void EnableFamiliars(bool enable_)
    {
        foreach (CombatPlayerActions player_ in players)
        {
            player_.DisableFamiliar(enable_);
        }
    }
    public CombatPlayerActions GetPlayer(int slot_)
    {
        if (slot_ > players.Length)
            slot_ = 0;

        return players[slot_];
    }
    public void MovePlayers(Transform newLocation_)
    {
        foreach (CombatPlayerActions player_ in players)
        {
            player_.transform.position = newLocation_.position;
            player_.transform.rotation = newLocation_.rotation;
        }
    }
}
