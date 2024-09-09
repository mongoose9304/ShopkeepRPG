using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectController : MonoBehaviour
{
    public RectTransform scrollObject;
    public float startPos;
    public float speed;
    public int target;
    public float distanceBetweenTargets;
    // Start is called before the first frame update
    void Start()
    {
        scrollObject.localPosition = new Vector3(startPos, 0, 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 x = new Vector3(target * distanceBetweenTargets, 0, 0);
        if(target * distanceBetweenTargets> scrollObject.localPosition.x)
        {
        scrollObject.localPosition = Vector3.MoveTowards(scrollObject.localPosition, x, speed*Time.deltaTime*Vector3.Distance(scrollObject.localPosition,x));

        }
        else if(target * distanceBetweenTargets < scrollObject.localPosition.x)
        {
            scrollObject.localPosition = Vector3.MoveTowards(scrollObject.localPosition, x, speed * Time.deltaTime * Vector3.Distance(scrollObject.localPosition, x));
        }
    }
}
