using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MoveObjectOnEnable : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;
    public float duration;
    private void OnEnable()
    {
        transform.localPosition = startPos;
        transform.DOLocalMove(endPos, duration, true);
    }
    
}
