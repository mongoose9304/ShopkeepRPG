using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class ChangeSprite : MonoBehaviour
{
    Image sprite;

    private void Awake() {
        sprite = GetComponent<Image>();
    }

    [YarnCommand("ChangeSprite")]
    public void Change(string spriteName) {
        
        Debug.Log("Change sprite to " + spriteName);
    }
    
}
