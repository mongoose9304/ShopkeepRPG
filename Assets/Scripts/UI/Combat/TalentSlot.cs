using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TalentSlot : MonoBehaviour
{
    public TextMeshProUGUI pointsInvestedText;
    public int points;
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
}
