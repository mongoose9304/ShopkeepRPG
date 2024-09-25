using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Room where a wheel will spin and activate a random effect (battle, money rain or treasure chest
/// </summary>
public class RouletteRumble : BasicRoom
{
    [Tooltip("Max number of enemies to spawn")]
    [SerializeField] private int maxEnemies;
    [Tooltip("REFERNCE to the wheel that spins to decide the outcome")]
    public RandomWheel Wheel;
    [Tooltip("REFERENCE to the object that will be activated once the spin has stopped")]
    [SerializeField] GameObject postSpinObject;
    [Tooltip("REFERENCE to the coin rain effect if the player hits a jackpot")]
    [SerializeField] CoinRain coinRain;
    [Tooltip("REFERENCE to the treasure chest that will be enabled if the player wins")]
    [SerializeField] TreasureChest chest;
    [Tooltip("REFERENCE to the object that will be activated once the spin has stopped")]
    [SerializeField] Image winImage;
    [Tooltip("REFERENCE images for each slot")]
    [SerializeField] Sprite[] slotSprites;
    [Tooltip("REFERENCE Spawns for the enemies")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    /// <summary>
    /// What will happen based on which slot has been picked
    /// </summary>
    public void RouletteEnd()
    {
       
        switch (Wheel.winSlot)
        {
            case 0:
                ActivateChest();
                    break;
            case 1:
                ActivateChest();
                break;
            case 2:
                StartFight();
                break;
            case 3:
                StartFight();
                break;
            case 4:
                ActivateJackpot();
                break;
            case 5:
                ActivateJackpot();
                break;
            case 6:
                StartFight();
                break;
            case 7:
                StartFight();
                break;
            case 8:
                ActivateChest();
                break;
            case 9:
                ActivateChest();
                break;
             default:
                StartFight();
                break;
        }
        Wheel.gameObject.SetActive(false);
        postSpinObject.SetActive(true);
    }
    private void StartFight()
    {
        Debug.Log("StartFight");
        winImage.sprite = slotSprites[0];
        CombatPlayerManager.instance.ReturnFamiliars();
        for(int i=0;i<maxEnemies;i++)
        {
            EnemyManager.instance.SpawnRandomEnemy(false, spawnPoints[i],null,DungeonManager.instance.GetEnemyLevel());
        }
    }
    private void ActivateChest()
    {
        Debug.Log("getChest");
        chest.gameObject.SetActive(true);
        winImage.sprite = slotSprites[1];
    }
    private void ActivateJackpot()
    {
        myDungeon = DungeonManager.instance.currentDungeon;
        coinRain.StartCoinRain(myDungeon.GetTreasureChestAmount()*3);
        winImage.sprite = slotSprites[2];
    }
    public override void StartRoomActivity()
    {
        Wheel.StartSpin();
    }
}
