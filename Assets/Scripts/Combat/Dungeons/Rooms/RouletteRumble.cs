using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteRumble : BasicRoom
{
    public RandomWheel Wheel;
    [SerializeField] GameObject postSpinObject;
    [SerializeField] CoinRain coinRain;
    [SerializeField] TreasureChest chest;
    [SerializeField] Image winImage;
    [SerializeField] Sprite[] slotSprites;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private int maxEnemies;

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
        Debug.Log("Jackpot");
        coinRain.StartCoinRain(myDungeon.GetTreasureChestAmount()*3);
        winImage.sprite = slotSprites[2];
    }
    public override void StartRoomActivity()
    {
        Wheel.StartSpin();
    }
}
