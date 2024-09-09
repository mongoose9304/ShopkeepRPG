using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TalentSlot : MonoBehaviour
{
    public TextMeshProUGUI pointsInvestedText;
    public string title;
    public string description;
    public int points;
    public TalentTree myTree;
   public void SetPoints(int points_)
    {
        pointsInvestedText.text = points_.ToString();
    }
    public void AddPoint()
    {
        points += 1;
        pointsInvestedText.text = points.ToString();
    }
    public void ResetPoints()
    {
        points = 0;
        pointsInvestedText.text = points.ToString();
    }
    public void ShowText()
    {
        myTree.SetText(description, title);
    }
}
