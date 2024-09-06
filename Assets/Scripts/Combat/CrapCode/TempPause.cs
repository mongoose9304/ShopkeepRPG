using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPause : MonoBehaviour
{
    public GameObject pauseObject;
    public bool isPaused;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("PauseGame"))
        {
            if (isPaused)
            {
                Time.timeScale = 1;
                pauseObject.SetActive(false);
                isPaused = false;
            }
            else
            {
                Time.timeScale = 0;
                pauseObject.SetActive(true);
                isPaused = true;
            }
        }
    }
}
