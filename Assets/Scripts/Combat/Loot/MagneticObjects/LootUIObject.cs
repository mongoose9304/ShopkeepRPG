using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;
using UnityEngine.UI;

public class LootUIObject : MonoBehaviour
{
   
    float currentLifetime;
    public float maxLifetime;
    public TextMeshProUGUI myText;
    public Sprite[] bgSprites;
    public Image myImage;
   [SerializeField] MMF_Player myPlayer;
   
    private void OnEnable()
    {
        currentLifetime = maxLifetime;
    }
    public void CreateUIObject(int amount,string name_,bool isNew=false,int bgToUse=0)
    {
        myText.text = name_ + " X" + amount.ToString();
        gameObject.SetActive(true);
        myImage.sprite = bgSprites[bgToUse];
        myPlayer.PlayFeedbacks();
        gameObject.transform.SetSiblingIndex(0);
        //Reset the positions to zero or its funky (objects at sacles like 2300 or far away like 24000Z -Rob)
        gameObject.transform.localScale = Vector3.one;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = new Quaternion(0,0,0,0);
    }

    private void Update()
    {
        currentLifetime -= Time.deltaTime;

        if (currentLifetime <= 0)
            gameObject.SetActive(false);
    }
}
