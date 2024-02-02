using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MonsterID : MonoBehaviour
{
    [Header("references")]
    [SerializeField] Image monsterImage;
    [SerializeField] TextMeshProUGUI name;
    [SerializeField] TextMeshProUGUI race;
    [SerializeField] Slider happinessSlider;
}
