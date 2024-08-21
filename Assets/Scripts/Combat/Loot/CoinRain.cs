using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRain : MonoBehaviour
{
    public float maxX;
    public float maxZ;
    [SerializeField] bool isRaining;
    [SerializeField] int rainDrops;
    int dropCount;
    [SerializeField] float maxTimeBetweenRainDrops;
    float currentTimeBetweenRainDrops;
    int dropValue;
    Vector3 originalPosition;
    // Update is called once per frame
    void Update()
    {
        if(isRaining)
        {
            currentTimeBetweenRainDrops -= Time.deltaTime;
            if(currentTimeBetweenRainDrops<=0)
            {
                transform.position = originalPosition;
                currentTimeBetweenRainDrops = maxTimeBetweenRainDrops;
                Vector3 position;
                position = transform.position;
                position += new Vector3(Random.Range(0, maxX), 0,Random.Range(0, maxZ));
                transform.position = position;
                CoinSpawner.instance_.CreateDemonCoins(dropValue, transform);
                dropCount += 1;
                if (dropCount >= rainDrops)
                    isRaining = false;
            }
        }
    }
    private void Start()
    {
        originalPosition = transform.position;
    }
    public void StartCoinRain(int value_)
    {
        isRaining = true;
        dropValue = value_ / rainDrops;
        dropCount = 0;
    }
}
