using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPause : MonoBehaviour
{
    public GameObject pauseObject;
    public List<GameObject> toggleOnPauseObjects = new List<GameObject>();
    public bool isPaused;
    public bool isInDialogue;
    public List<GameObject> objectsToDisableDuringDialogue = new List<GameObject>();
    // Update is called once per frame
    public static TempPause instance;
    private void Awake()
    {
        instance = this;
    }
 
    public void TogglePause()
    {
        if (isInDialogue)
            return;
        if (isPaused)
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            isPaused = false;
            foreach (GameObject obj in toggleOnPauseObjects)
            {
                obj.SetActive(true);
            }
            pauseObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            Time.fixedDeltaTime = 0;
            isPaused = true;
            foreach (GameObject obj in toggleOnPauseObjects)
            {
                obj.SetActive(false);
            }
            pauseObject.SetActive(true);
        }
    }


    public void PauseForDialogue()
    {
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        isInDialogue = true;
        isPaused = true;
        foreach(GameObject obj in objectsToDisableDuringDialogue)
        {
            obj.SetActive(false);
        }
    }
    public void UnPauseForDialogue()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        isPaused = false;
        isInDialogue = false;
        foreach (GameObject obj in objectsToDisableDuringDialogue)
        {
            obj.SetActive(true);
        }
    }
}
