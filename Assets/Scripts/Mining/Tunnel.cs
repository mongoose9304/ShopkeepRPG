using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tunnel : InteractableObject
{
    public Transform teleportLocation;
    public GameObject objectToSetActive;
    public GameObject objectToSetInactive;
    [SerializeField] float maxHoldDuration;
    [SerializeField] float currentHoldDuration;
    public MMProgressBar myUIBar;
    private void Update()
    {
        if(currentHoldDuration>=maxHoldDuration)
        {
            Use();
            currentHoldDuration = 0;
        }
        if(currentHoldDuration>0)
            currentHoldDuration -= Time.deltaTime;
        if (currentHoldDuration < 0)
            currentHoldDuration = 0;

        AdjustBar();
    }
    public void Teleport(GameObject obj_)
    {
        obj_.transform.position = teleportLocation.position;
    }
    public override void Interact()
    {
        currentHoldDuration += Time.deltaTime*2;
    }
    public void Use()
    {
        Teleport(GameObject.FindGameObjectWithTag("Player"));
        if (objectToSetActive)
            objectToSetActive.SetActive(true);
        if (objectToSetInactive)
            objectToSetInactive.SetActive(false);
        Debug.Log("Interact");
    }
    private void AdjustBar()
    {
        myUIBar.UpdateBar01(currentHoldDuration / maxHoldDuration);
    }
}
