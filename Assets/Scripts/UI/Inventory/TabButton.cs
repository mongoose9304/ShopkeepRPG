using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour
{
    public TabGroup tabGroup;
    public Image background;

    // Start is called before the first frame update
    void Start()
    {
        background = GetComponent<Image>();
        tabGroup.SubToList(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
