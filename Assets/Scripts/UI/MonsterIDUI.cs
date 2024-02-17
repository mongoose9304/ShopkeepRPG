using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//controls the UI page for monster ID. Examples include setting up the UI based on what monster is selected and displaying all that monsters UI
public class MonsterIDUI : MonoBehaviour
{
    [Header("Debug")]
    public BasicMonsterData testData;
    private void Start()
    {
        PopulatePage(testData);
    }


    [Header("references")]
    [SerializeField] Image monsterImageReference;
    [SerializeField] Image monsterElementReference;
    [SerializeField] Sprite[] elementSprites;
    [SerializeField] TextMeshProUGUI monsterNameTextField;
    [SerializeField] TextMeshProUGUI monsterRaceTextField;
    [SerializeField] TextMeshProUGUI monsterLevelTextField;
    [SerializeField] TextMeshProUGUI monsterContractTextField;
    [SerializeField] TextMeshProUGUI decriptionText;
    [SerializeField] TextMeshProUGUI loreTextField;

    //
    [Header("Stat References")]
    [SerializeField] TextMeshProUGUI attackTextField;
    [SerializeField] TextMeshProUGUI defenceTextField;
    [SerializeField] TextMeshProUGUI speedTextField;
    [SerializeField] TextMeshProUGUI healthTextField;
    [SerializeField] TextMeshProUGUI workEthicTextField;
    [Header("WorkSkills References")]
    [SerializeField] Image[] WorkImages;
    
    [Header("WorkDays References")]
    [SerializeField] Image mondayReference;
    [SerializeField] Image tuesdayReference;
    [SerializeField] Image wednesdayReference;
    [SerializeField] Image thursdayReference;
    [SerializeField] Image fridayReference;
    [SerializeField] Image saturdayReference;
    [SerializeField] Image sundayReference;




    public void PopulatePage(BasicMonsterData data_)
    {
        //Set the Work images color based on the monster's ability to perform them. 
        for(int i = 0; i < data_.jobs.Length; i++)
        {
            if(data_.jobs[i])
            {
                WorkImages[i].color = Color.green;
            }
            else
            {
                WorkImages[i].color = Color.red;
            }
        }
        for (int i = 0; i < data_.specializations.Length; i++)
        {
            if (data_.jobs[i])
            {
                WorkImages[i].color = Color.yellow;
            }
        }
        //Set up the UI with the monsters data
        monsterImageReference.sprite = data_.monsterSprite;
        monsterRaceTextField.text = data_.originalName;
        loreTextField.text = data_.lore;
        SetElement(data_.element);



    }
    //sets the Monsters element based on input data
    private void SetElement(Element el_)
    {
        switch (el_)
        {
            case Element.Fire:
            {
                    monsterElementReference.sprite = elementSprites[0];
                    monsterElementReference.color = Color.red;
            }
            break;
            case Element.Water:
            {
                    monsterElementReference.sprite = elementSprites[1];
                    monsterElementReference.color = Color.blue;
                }
            break;
            case Element.Earth:
            {
                    monsterElementReference.sprite = elementSprites[2];
                    monsterElementReference.color = Color.yellow;
                }
            break;
            case Element.Air:
            {
                    monsterElementReference.sprite = elementSprites[3];
                    monsterElementReference.color = Color.green;
                }
            break;
            case Element.Neutral:
            {
                    monsterElementReference.sprite = elementSprites[4];
                    monsterElementReference.color = Color.white;
                }
            break;
        }
    }
}
