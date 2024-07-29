using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberLevelManager : MonoBehaviour
{
    public static LumberLevelManager instance;
    public LumberLevel currentLevel;
    public LumberLevel nextLevel;
    private void Awake()
    {
        instance = this;
    }
}
