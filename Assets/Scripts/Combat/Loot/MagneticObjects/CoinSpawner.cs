using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class CoinSpawner : MonoBehaviour
{
    public MMMiniObjectPooler demonCoinPool;
    public MMMiniObjectPooler regularCoinPool;
    public int[] demonCoins;
    public int[] regularCoins;
    public static CoinSpawner instance_;
    int[] temp;
    int[] tempB;

    private void Start()
    {
        instance_ = this;
        temp = demonCoins;
    }

    public void CreateDemonCoins(int value_,Transform location_)
    {
         temp = MakeDemonChange(value_);
        for ( int i= 0;i<temp.Length;i++)
        {
           
            if (temp[i] == 0)
                continue;
            //for(int x=0;x<temp[i];x++)
           // {
                GameObject coinObject = demonCoinPool.GetPooledGameObject();
                coinObject.transform.position = location_.position;
                coinObject.transform.rotation = location_.rotation;
                coinObject.transform.position += new Vector3(Random.Range(0,2), Random.Range(0, 2), Random.Range(0, 2));
                coinObject.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0, 0.25f), 4, Random.Range(0, 0.25f)), ForceMode.VelocityChange);
                coinObject.GetComponent<DemonCoin>().SetUpCoin(demonCoins[i]);
                coinObject.SetActive(true);
           // }
            
        }
    }
    public void CreateRegularCoins(int value_, Transform location_)
    {
        temp = MakeRegularChange(value_);
        for (int i = 0; i < temp.Length; i++)
        {

            if (temp[i] == 0)
                continue;
            for (int x = 0; x < temp[i]; x++)
            {
                DemonCoin coin = regularCoinPool.GetPooledGameObject().GetComponent<DemonCoin>();
                coin.transform.position = location_.position;
                coin.transform.rotation = location_.rotation;
                coin.transform.position += new Vector3(Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2));
                coin.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0, 0.25f), 4, Random.Range(0, 0.25f)), ForceMode.VelocityChange);
                coin.SetUpCoin(regularCoins[i]);
            }

        }
    }
    private int[] MakeDemonChange(int amount_)
    {
        int[] dp = new int[amount_ + 1];
        int[] usedCoins = new int[amount_ + 1];

        for (int i = 1; i <= amount_; i++)
        {
            dp[i] = int.MaxValue; // Initialize with a large value
            foreach (int coin in demonCoins)
            {
                if (i - coin >= 0 && dp[i - coin] + 1 < dp[i])
                {
                    dp[i] = dp[i - coin] + 1;
                    usedCoins[i] = coin;
                }
            }
        }

        // Reconstruct the coin combination
        int[] result = new int[demonCoins.Length];
        int remaining = amount_;
        while (remaining > 0)
        {
            int coin = usedCoins[remaining];
            for (int i = 0; i < demonCoins.Length; i++)
            {
                if (demonCoins[i] == coin)
                {
                    result[i]++;
                    break;
                }
            }
            remaining -= coin;
        }

        return result;
    }
    private int[] MakeRegularChange(int amount_)
    {
        int[] dp = new int[amount_ + 1];
        int[] usedCoins = new int[amount_ + 1];

        for (int i = 1; i <= amount_; i++)
        {
            dp[i] = int.MaxValue; // Initialize with a large value
            foreach (int coin in regularCoins)
            {
                if (i - coin >= 0 && dp[i - coin] + 1 < dp[i])
                {
                    dp[i] = dp[i - coin] + 1;
                    usedCoins[i] = coin;
                }
            }
        }

        // Reconstruct the coin combination
        int[] result = new int[regularCoins.Length];
        int remaining = amount_;
        while (remaining > 0)
        {
            int coin = usedCoins[remaining];
            for (int i = 0; i < regularCoins.Length; i++)
            {
                if (regularCoins[i] == coin)
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
