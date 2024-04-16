using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CoinSpawner : MonoBehaviour
{
    public GameObject demonCoin;
    public int[] coins;
    public static CoinSpawner instance_;
    int[] temp;

    private void Start()
    {
        instance_ = this;
        temp = coins;
    }

    public void CreateCoins(int value_,Transform location_)
    {
        temp = MakeChange(value_);
        for ( int i= 0;i<temp.Length;i++)
        {
            Debug.Log("Change i= " + i);
            if (temp[i] == 0)
                continue;
            for(int x=0;x<temp[i];x++)
            {
              DemonCoin coin=GameObject.Instantiate(demonCoin, location_.position, location_.rotation).GetComponent<DemonCoin>();
                coin.transform.position += new Vector3(Random.Range(0,2), Random.Range(0, 2), Random.Range(0, 2));
                coin.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0, 0.25f), 4, Random.Range(0, 0.25f)), ForceMode.VelocityChange);
                coin.SetUpCoin(coins[i]);
            }
            
        }
    }
    private int[] MakeChange(int amount_)
    {
        int[] dp = new int[amount_ + 1];
        int[] usedCoins = new int[amount_ + 1];

        for (int i = 1; i <= amount_; i++)
        {
            dp[i] = int.MaxValue; // Initialize with a large value
            foreach (int coin in coins)
            {
                if (i - coin >= 0 && dp[i - coin] + 1 < dp[i])
                {
                    dp[i] = dp[i - coin] + 1;
                    usedCoins[i] = coin;
                }
            }
        }

        // Reconstruct the coin combination
        int[] result = new int[coins.Length];
        int remaining = amount_;
        while (remaining > 0)
        {
            int coin = usedCoins[remaining];
            for (int i = 0; i < coins.Length; i++)
            {
                if (coins[i] == coin)
                {
                    result[i]++;
                    break;
                }
            }
            remaining -= coin;
        }

        return result;
    }
}
