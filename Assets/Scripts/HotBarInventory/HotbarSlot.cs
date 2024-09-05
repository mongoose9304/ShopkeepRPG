using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarSlot : MonoBehaviour
{
    public GameObject highlightObject;
    public GameObject engagedObject;
    public void SetHighlighted()
    {
        highlightObject.SetActive(true);
    }
    public void SetUnHighlighted()
    {
        highlightObject.SetActive(false);
        engagedObject.SetActive(false);
    }
    public float Use()
    {
        engagedObject.SetActive(true);
        return 1;
    }
}
