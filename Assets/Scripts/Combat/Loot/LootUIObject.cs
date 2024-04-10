using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Tools;
public class LootUIObject : MonoBehaviour
{
   
    float currentLifetime;
    public float maxLifetime;
    public TextMeshProUGUI myText;
   
    private void OnEnable()
    {
        currentLifetime = maxLifetime;
    }
    public void CreateUIObject(int amount,string name_,bool isNew=false)
    {
        myText.text = name_ + " X" + amount.ToString();
        gameObject.SetActive(true);
    }

    private void Update()
    {
        currentLifetime -= Time.deltaTime;

        if (currentLifetime <= 0)
            gameObject.SetActive(false);
    }
}
