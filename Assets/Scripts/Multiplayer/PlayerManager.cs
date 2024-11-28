using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private List<PlayerController> players = new List<PlayerController>();
    [SerializeField]
    private Transform playerSpawn;
    [SerializeField]
    private List<LayerMask> playerLayers;
    private PlayerInputManager playerInputManager;

        private void Awake()
    {
        playerInputManager = FindObjectOfType<PlayerInputManager>();
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= AddPlayer;
    }
    public void AddPlayer(PlayerInput player)
    {
        Debug.Log("PlayerAdded");
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
}
