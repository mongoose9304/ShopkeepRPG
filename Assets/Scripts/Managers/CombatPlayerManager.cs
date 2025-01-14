using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatPlayerManager : MonoBehaviour
{
    public static CombatPlayerManager instance;
    [SerializeField] CombatPlayerActions[] players;
    [SerializeField] CombatCoopFamiliar familiarPlayer;
    public bool coopPlayer;
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
        if(coopPlayer)
        {
            return;
        }
        foreach (CombatPlayerActions player_ in players)
        {
            player_.DisableFamiliar(enable_);
        }
    }
    public void ConnectOtherPlayer()
    {
        coopPlayer = true;
        foreach (CombatPlayerActions player_ in players)
        {
            player_.DisableFamiliar(false);
            player_.coopPlayer = true;
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
        familiarPlayer.transform.position = newLocation_.position;
        familiarPlayer.transform.rotation = newLocation_.rotation;
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
    public void TeleportCoopPlayerToMainPlayer()
    {
        NavMeshHit hit;
        Vector3 newPos = familiarPlayer.transform.position;
        if (NavMesh.SamplePosition(players[0].transform.position, out hit, 3.0f, NavMesh.AllAreas))
        {
            newPos = hit.position;
            newPos.y = familiarPlayer.transform.position.y;
            familiarPlayer.transform.position = newPos;
        }
    }
    public void TeleportMainPlayerToCoopPlayer()
    {
        NavMeshHit hit;
        Vector3 newPos = players[0].transform.position;
        if (NavMesh.SamplePosition(familiarPlayer.transform.position, out hit, 3.0f, NavMesh.AllAreas))
        {
            newPos = hit.position;
            newPos.y = players[0].transform.position.y;
            players[0].transform.position = newPos;
        }
    }
}
