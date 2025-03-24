using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DungeonFloorTimer : MonoBehaviour
{
    [SerializeField]
    private float timeLimit = 300f;
    private float currentTimer;

    public TextMeshProUGUI timerText;

    public delegate void TimerEnded();
    public event TimerEnded OnTimerEnd;

    private void Start()
    {
        StartTimer();
    }

    private void Update()
    {
        currentTimer -= Time.deltaTime;

        if (currentTimer <= 0)
        {
            currentTimer = 0;
            if (OnTimerEnd != null)
                OnTimerEnd.Invoke();
        }

        if (timerText != null)
            UpdateTimerUI();
    }

    public void StartTimer()
    {
        currentTimer = timeLimit;
    }

    public void ResetTimer()
    {
        currentTimer = timeLimit;
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTimer / 60F);
        int seconds = Mathf.FloorToInt(currentTimer - minutes * 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
