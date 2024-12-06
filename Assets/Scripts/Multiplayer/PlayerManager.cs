using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Manages players joining the game
/// </summary>
public class PlayerManager : MonoBehaviour
{
    [Tooltip("The singleton instance of this class")]
    public static PlayerManager instance;
    [Tooltip("All the players loaded in the game")]
    private List<PlayerController> players = new List<PlayerController>();
    [SerializeField]
    [Tooltip("REFERENCE to the layers used for player virtual cameras")]
    private List<LayerMask> playerLayers;
    [Tooltip("REFERENCE to the player input manager")]
    private PlayerInputManager playerInputManager;

        private void Awake()
    {
        if (!instance)
        {
            playerInputManager = FindObjectOfType<PlayerInputManager>();
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= AddPlayer;
    }
    /// <summary>
    /// Disable player 2 for a bit for xutscenes and such
    /// </summary>
    public void TemporaryDisablePlayer2()
    {
        if(players.Count>1)
        {
            players[1].gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// bring player 2 back after cutscens and such
    /// </summary>
    public void BringPlayer2Back()
    {
        if (players.Count > 1)
        {
            players[1].gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// Add a player to the game
    /// </summary>
    public void AddPlayer(PlayerInput player)
    {
        Debug.Log("PlayerAdded");
        if(players.Contains(player.GetComponent<PlayerController>()))
        {
            return;
        }
        players.Add(player.GetComponent<PlayerController>()); 
        int layerToAdd = (int)Mathf.Log(playerLayers[players.Count - 1].value, 2);
        player.GetComponentInChildren<Camera>().cullingMask |= 1 << layerToAdd;
        player.GetComponent<PlayerController>().input = player;
        if (players.Count==1)
        SceneSpecificPlayerManager.instance.CreatePlayer1(players[0]);
        else
        {
            SceneSpecificPlayerManager.instance.CreatePlayer2(players[1]);
        }

    }
    /// <summary>
    /// Return a ref to all players
    /// </summary>
    public List<PlayerController> GetPlayers()
    {
        return players;
    }
}
