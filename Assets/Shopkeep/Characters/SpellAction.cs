using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Actions/SpellAction")]
public class UseSpell : CharacterAction {
    SpellComponent spellComponent;
    public Shopkeeper.Spell spell;

    public override bool Enter(GameObject character) {
        if (spellComponent == null) {
            spellComponent = character.GetComponent<SpellComponent>();
            return false;
        }

        if (spell == null) {
            return false;
        }

        spellComponent.CastSpell(spell);

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