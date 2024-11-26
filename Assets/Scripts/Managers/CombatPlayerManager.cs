using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPlayerManager : MonoBehaviour
{
    public static CombatPlayerManager instance;
    [SerializeField] CombatPlayerActions[] players;
    [SerializeField] CombatCoopFamiliar familiarPlayer;
    public Hotbar playerHotbar;
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
    public void ResetPlayerSosChances()
    {
        foreach (CombatPlayerActions player_ in players)
        {
            player_.combatMovement.timesYouHaveDied = 0;
        }
    }
    public void AddModiferToAllPlayers(ExternalModifier mod_)
    {
        foreach (CombatPlayerActions player_ in players)
        {
            foreach (EquipModifier modx in mod_.myModifiers)
                player_.combatMovement.AddExternalMod(modx);
        }
    }
    public void RemoveModiferFromAllPlayers(ExternalModifier mod_)
    {
        foreach (CombatPlayerActions player_ in players)
        {
            foreach (EquipModifier modx in mod_.myModifiers)
                player_.combatMovement.RemoveExternalMod(modx);
        }
    }
    public void LevelUp()
    {
        foreach (CombatPlayerActions player_ in players)
        {
            player_.combatMovement.LevelUp();
        }
    }
    public int GetExpToNextLevel()
    {
        return players[0].combatMovement.GetExpToNextLevel();
    }
    public void HealPlayer(float healAmount)
    {
        players[0].combatMovement.HealthPickup(healAmount);
    }
    public void RestorePlayerMana(float healAmount)
    {
        players[0].combatMovement.ManaPickup(healAmount);
    }
    public void RemoveInteractableObject(GameObject obj)
    {
        players[0].combatMovement.RemoveInteractableObject(obj);
        familiarPlayer.RemoveInteractableObject(obj);
    }
}
