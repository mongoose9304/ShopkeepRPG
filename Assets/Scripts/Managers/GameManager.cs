using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; private set;}

    /*
    [SerializeField]
    private GaneState startState;

    publc GameState GameState {get; private set;}

    */
    public InventoryManager inventoryManager;
    public PlayerManager playerManager;
    public UIManager uIManager;

    void Awake(){
        if(instance != null && instance != this){
            Destroy(gameObject);
            return;
        }
        else{
            instance = this;
            DontDestroyOnLoad(this);
        }


        // set up the states
    }
}
