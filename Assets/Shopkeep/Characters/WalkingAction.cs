using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Actions/WalkingAction")]
public class Walking : CharacterAction {
    Shopkeeper.CharacterWalking walkingComponent;

    public override bool Enter(GameObject character) {
        if (walkingComponent == null) {
            walkingComponent = character.GetComponent<Shopkeeper.CharacterWalking>();
            Debug.Log(character.name + " doesn't have a walking component");
            return false;
        }

        Vector2 input = inputContext.ReadValue<Vector2>().normalized;
        walkingComponent.moveDirection = new Vector3(input.x, 0.0f, input.y);
        return true;
    }

    public override bool Exit(GameObject character) {
        completed = true;
        return true;
    }

    public override IEnumerator actionDuration(GameObject character) {
        yield return new WaitForSeconds(0.1f);
        Exit(character);
    }
}
