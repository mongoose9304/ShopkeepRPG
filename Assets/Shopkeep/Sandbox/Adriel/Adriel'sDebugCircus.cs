using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shopkeeper;

public class AdrielsDebugCircus : MonoBehaviour
{
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
}
