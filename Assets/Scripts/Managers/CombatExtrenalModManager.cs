using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatExtrenalModManager : MonoBehaviour
{
    public static CombatExtrenalModManager instance;
    public List<ExternalModifier> externalMods = new List<ExternalModifier>();
    private void Awake()
    {
        instance = this;
    }
    public ExternalModifier GetModByName(string name_)
    {
        foreach(ExternalModifier mod_ in externalMods)
        {
            if(mod_.modsName==name_)
            {
                return mod_;
            }
        }
        return null;
    }
    public void AddModToAllPlayers(string name_)
    {
        CombatPlayerManager.instance.AddModiferToAllPlayers(GetModByName(name_));
    }
    public void RemoveModFromAllPlayers(string name_)
    {
        CombatPlayerManager.instance.RemoveModiferFromAllPlayers(GetModByName(name_));
    }
}
