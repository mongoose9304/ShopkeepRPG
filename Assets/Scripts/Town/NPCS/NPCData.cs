using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "ScriptableObjects/NPCData")]
public class NPCData : ScriptableObject
{
    // Store all non-changing data of npc
    public string NPCname;

    // preffered atmosphere i.e. cheap, fancy, evil, cozy, etc. for now save as string
    public string atmosphere;
    
    // array for liked items (sways npc to buy them) and adds friendship/romance points if gifted. (and not a generic npc)
    public List<string> likedItems = new List<string>();

    // array for disliked items (npcs wont want to buy these types), removes friendship and romance points if gifted.
    public List<string> dislikedItems = new List<string>();

    // buy percentage
    public float buyPercentage; // ex. 0.2 = 120% is the max they'll haggle to, we could also do a sweetspot randomized in the npc code.

    // budget range for npc to buy items (might work this into the normal npc script, the budget should increase over time)
    public int budgetMin; // starting budget for the game (this is mainly to control the  difficulty, can be removed)
    public int budgetMax; // by year 1-2 npcs should hit their max budget

    // npc's active time of day, ex. noon, evening, night
    public List<string> activeTime = new List<string>();

    // npc's home
    public string home;
}
