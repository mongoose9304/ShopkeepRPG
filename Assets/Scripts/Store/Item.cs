using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Start and Update are not needed.
    public enum ItemType
    {
        Weapon,
        Armor,
        Accessory,
        Material,
        Treasure,
        Trash
    }

    //TODO: Add more to items, i.e. if there should be damage stat / armor value
    public string itemName;
    public string description;
    public int basePrice;
    public ItemType type;

    /*
    // thought of more rpg style stats, just placeholders for now.
    public Sprite sprite;
    public int weight;
    public int value;
    public int damage;
    public int armor;
    public int health;
    public int mana;
    public int strength;
    public int dexterity;
    public int intelligence;
    public int wisdom;
    public int charisma;
    public int level;
    public int rarity;
    public int itemLevel;
    public int itemRarity;
    public int itemType;
    public int itemSubType; 
    */
}
