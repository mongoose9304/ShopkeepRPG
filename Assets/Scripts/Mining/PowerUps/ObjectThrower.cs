using Blobcreate.ProjectileToolkit;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThrower : MonoBehaviour
{
    public List<GameObject> objectsToThrow = new List<GameObject>();
    public List<Transform> objectTargets = new List<Transform>();
    public List<Transform> objectStartPositions = new List<Transform>();
    public AudioClip throwAudio;
    public void ThrowAllObjects()
    {
        for (int i = 0; i < objectsToThrow.Count; i++)
        {
            objectsToThrow[i].transform.position = objectStartPositions[i].position;
            objectsToThrow[i].SetActive(true);
            objectsToThrow[i].GetComponent<Rigidbody>().AddForce(Projectile.VelocityByA(objectsToThrow[i].transform.position, objectTargets[i].transform.position, -0.1f), ForceMode.VelocityChange);
        }
        if(throwAudio)
        {
            MMSoundManager.Instance.PlaySound(throwAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
     false, 1.0f, 0, false, 0, 1, null, false, null, null, 1.0f, 0, 0.0f, false, false, false, false, false, false, 128, 1f,
     1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
        }
    }
}
