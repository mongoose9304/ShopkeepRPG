using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shopkeeper;
using UnityEngine.InputSystem;

public class AdrielsDebugCircus : MonoBehaviour
{
    public GameObject playerPrefab;
    public Spell MeleeSpell;
    public Spell ProjectileSpell;

    // Start is called before the first frame update
    void Start()
    {
        Shopkeeper.Stat test = new Shopkeeper.Stat(4.0f);
        test.modifier.AddListener(ModifyValue1);
        test.modifier.AddListener(ModifyValue2);

        Debug.Log(test.GetValue());
    }

    public void ModifyValue1(FloatWrapper v) {
        v.value *= 2.0f;
    }
    public void ModifyValue2(FloatWrapper v) {
        v.value += 2.0f;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void MeleeCallback(InputAction.CallbackContext context) {
        Debug.Log("Meleee");
        SpellComponent SP = playerPrefab.GetComponent<SpellComponent>();
        if (SP == null) return;

        SP.CastSpell(MeleeSpell);
    }

    public void ProjectileCallback(InputAction.CallbackContext context) {
        Debug.Log("Projectile");
        SpellComponent SP = playerPrefab.GetComponent<SpellComponent>();

        if (SP == null) return;

        SP.CastSpell(ProjectileSpell);
    }
}
