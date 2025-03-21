using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    public void StartLifeCoroutine(float duration) {
        StartCoroutine(LifeCoroutine(duration));
    }

    IEnumerator LifeCoroutine(float duration) {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
