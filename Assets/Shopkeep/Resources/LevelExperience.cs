using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelExperience : MonoBehaviour
{
    private int level;
    private float xp;
    private float xpToLevelUp;
    [SerializeField]
    public UnityEvent onLevelUp;

    // Start is called before the first frame update
    void Start()
    {
        xp = 0;
        level = 0;
        xpToLevelUp = 10;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddXP(float amount) 
    {
        xp += amount;
        if (xp > xpToLevelUp)
        {
            xp -= xpToLevelUp;
            level += 1;
            CalculateNewXPMax();
            onLevelUp.Invoke();
        }
    }

    private void CalculateNewXPMax()
    {
        // Increase by 20% for now, can add a more complicated scaling formula later
        xpToLevelUp = xpToLevelUp * 1.2f;
    }

    public int GetLevel()
    {
        return level;
    }
}
