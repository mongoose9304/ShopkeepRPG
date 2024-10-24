using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The teleporter that connects mining levels together 
/// </summary>
public class Tunnel : InteractableObject
{
    [Tooltip("The location to send the player")]
    public Transform teleportLocation;
    [Tooltip("Start the game if this is the end of the tutorial")]
    public bool tutEndTunnel;
    [Tooltip("Use this for telports between mining levels that you want to keep the previous level's data. Used for tutorials and odd situations")]
    public bool doNotChangeMiningLevels;
    [Tooltip("The object to activate on teleporting, usually the next level")]
    public GameObject objectToSetActive;
    [Tooltip("The object to deactivate on teleporting, usually the previous level")]
    public GameObject objectToSetInactive;
    [Tooltip("The time you must hold the interact button before the tunnel will teleport a player")]
    [SerializeField] float maxHoldDuration;
    float currentHoldDuration;
    [Tooltip("REFERNCE to the UI bar that fills up as held")]
    public MMProgressBar myUIBar;
    public AudioClip tunnelAudio;
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
    /// <summary>
    /// Teleport an object to the target location 
    /// </summary>
    public void Teleport(GameObject obj_)
    {
        obj_.transform.position = teleportLocation.position;
    }
    public override void Interact(GameObject interactingObject_ = null)
    {
        currentHoldDuration += Time.deltaTime*2;
    }
    /// <summary>
    /// Will teleport the player and set the objects active/inactive as necessary 
    /// </summary>
    public void Use()
    {
        PlayAudio();
        if(tutEndTunnel)
        {
            TutorialManager.instance_.EndTutorial();
            teleportLocation = MiningManager.instance.currentLevel.startLocation;
            Teleport(GameObject.FindGameObjectWithTag("Player"));
            objectToSetActive = MiningManager.instance.currentLevel.gameObject;
            if (objectToSetActive)
            {
                objectToSetActive.SetActive(true);
                if (objectToSetActive.TryGetComponent<MiningLevel>(out MiningLevel lv))
                {
                    lv.StartLevel();
                }
            }
            if (objectToSetInactive)
                objectToSetInactive.SetActive(false);
            return;
        }
        Teleport(GameObject.FindGameObjectWithTag("Player"));
        if (objectToSetActive)
        {
            if (!doNotChangeMiningLevels)
            {
                objectToSetActive.SetActive(true);
                if (objectToSetActive.TryGetComponent<MiningLevel>(out MiningLevel lv))
                {
                    lv.StartLevel();
                }
            }
            else
            {
                objectToSetActive.SetActive(true);
            }
        }
        if (objectToSetInactive)
            objectToSetInactive.SetActive(false);
        Debug.Log("Interact");
    }
    /// <summary>
    /// Adjusts the UI bar based on how long you hold down for
    /// </summary>
    private void AdjustBar()
    {
        myUIBar.UpdateBar01(currentHoldDuration / maxHoldDuration);
    }
    public void PlayAudio()
    {
        if (tunnelAudio)
            MMSoundManager.Instance.PlaySound(tunnelAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
        false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
        1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
}
