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
    void Update()
    {
        if (Input.GetButtonDown("PauseGame"))
        {
            if (isPaused)
            {
                Time.timeScale = 1;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                pauseObject.SetActive(false);
                isPaused = false;
                foreach(GameObject obj in toggleOnPauseObjects)
                {
                    obj.SetActive(true);
                }
            }
            else
            {
                Time.timeScale = 0;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                pauseObject.SetActive(true);
                isPaused = true;
                foreach (GameObject obj in toggleOnPauseObjects)
                {
                    obj.SetActive(false);
                }
            }
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
