using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChaseTimeBar : MonoBehaviour
{
    public Image foreground;
    public Image background;
    public Canvas barHolder;
    private Camera mainCamera;

    // Start is called before the first frame update
    public void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null) {
            Debug.LogError("Camera is missing Trapchest");
        }
        // Add validation checks
        if (foreground == null)
        {
            Debug.LogError("Foreground is missing Trapchest");
        }
        if (background == null)
        {
            Debug.LogError("Background is missing Trapchest" + gameObject.name);
        }
    }
    public void UpdateTheVar(float currentTime, float chaseTime) {
        foreground.fillAmount = 1 - (currentTime / chaseTime);
        Vector3 cameraDir = mainCamera.transform.position - barHolder.transform.position;
        Vector3 currentRotation = barHolder.transform.rotation.eulerAngles;

        // Modify specific axes
        currentRotation.x = 42.0f;
        currentRotation.z = 90.0f;
        float uRot = Quaternion.LookRotation(cameraDir).eulerAngles.y;
        // Apply the modified rotation
        barHolder.transform.rotation = Quaternion.Euler(-42.0f, uRot, 90.0f );

    }
}
