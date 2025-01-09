using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// What happens when player join in the combat scene
/// </summary>
public class CombatSceneSpecificPlayerManager : SceneSpecificPlayerManager
{
    public CombatPlayerMovement player1;
    public CombatCoopFamiliar player2;
    
    public override void CreatePlayer1(PlayerController controller)
    {
        foreach (GameObject obj in objectsToDisableWhenPlayer1Joins)
        {
            obj.SetActive(false);
        }
        foreach (Canvas canv in player1Canvases)
        {
            canv.worldCamera = controller.myCam;
        }
        player1.SetUpControls(controller.input);
        player1.combatActions.ChangeFamiliar(PlayerManager.instance.currentFamiliar);
        player1.gameObject.SetActive(true);
    }

        public override void CreatePlayer2(PlayerController controller)
        {

        player2.SetUpControls(controller.input);
        player2.ChangeFamiliar(PlayerManager.instance.currentFamiliar);
        player2.transform.position = player1.transform.position;
        player2.gameObject.SetActive(true);
        CombatPlayerManager.instance.ConnectOtherPlayer();
        DungeonManager.instance.in2PlayerMode = true;
        }
}
