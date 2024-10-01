using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ScaleObjectOnEnable : MonoBehaviour
{
    public Vector3 startScale;
    public Vector3 endScale;
    public float duration;
    private void OnEnable()
    {
        transform.localScale = startScale;
        transform.DOScale(endScale,duration);
    }
}
