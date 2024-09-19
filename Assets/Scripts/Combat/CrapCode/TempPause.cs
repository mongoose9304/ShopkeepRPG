using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPause : MonoBehaviour
{
    public GameObject pauseObject;
    public List<GameObject> toggleOnPauseObjects = new List<GameObject>();
    public bool isPaused;
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
                pauseObject.SetActive(true);
                isPaused = true;
                foreach (GameObject obj in toggleOnPauseObjects)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}
