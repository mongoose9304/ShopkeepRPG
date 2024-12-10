using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    public string levelName;
    public void LoadMyLevel()
    {
        LoadingManager.instance.LoadScene(levelName);
    }
}
