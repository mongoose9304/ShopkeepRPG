using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Mostly virtual class that has all the functionality of what happens when players join in. Usually involving taking over specifc objects or enabling/disabling canvases
/// </summary>
public class SceneSpecificPlayerManager : MonoBehaviour
{
    [Tooltip("The singleton instance of this class")]
    public static SceneSpecificPlayerManager instance;
    [Tooltip("REFERENCE to all the objects to disable if player 1 joins")]
    public List<GameObject> objectsToDisableWhenPlayer1Joins = new List<GameObject>();
    [Tooltip("REFERENCE to all the objects to enable if player 2 joins")]
    public List<GameObject> objectsToEnableWhenPlayer2Joins = new List<GameObject>();
    [Tooltip("REFERENCE to all the canvases assigned to player 1's camera")]
    public List<Canvas> player1Canvases = new List<Canvas>();
    [Tooltip("REFERENCE to all the canvases assigned to player 2's camera")]
    public List<Canvas> player2Canvases = new List<Canvas>();
    private void Awake()
    {
        instance = this;
    }
    protected virtual void Start()
    {
        if (PlayerManager.instance.GetPlayers().Count > 0)
        {
            CreatePlayer1(PlayerManager.instance.GetPlayers()[0]);
        }
        if (PlayerManager.instance.GetPlayers().Count > 1)
        {
            CreatePlayer2(PlayerManager.instance.GetPlayers()[1]);
        }
    }
    /// <summary>
    /// Create player 1 and assign their character
    /// </summary>
    public virtual void CreatePlayer1(PlayerController controller)
    {
        foreach(GameObject obj in objectsToDisableWhenPlayer1Joins)
        {
            obj.SetActive(false);
        }
        foreach(Canvas canv in player1Canvases)
        {
            canv.worldCamera = controller.myCam;
        }
    }
    /// <summary>
    /// Create player 2 and assign their character
    /// </summary>
    public virtual void CreatePlayer2(PlayerController controller)
    {

    }
}
