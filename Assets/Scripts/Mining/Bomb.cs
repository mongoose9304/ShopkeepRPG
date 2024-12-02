using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
/// <summary>
/// The bombs players and enemies drop that explode after a short duration
/// </summary>
public class Bomb : MonoBehaviour
{
    [Tooltip("Length of time before bomb explodes")]
    public float maxDetonationTime;
    float currentDetonationTime;
    [Tooltip("how many squares the explosion will cover")]
    public int range = 1;
    [Tooltip("What will stop or limit an explosion's path")]
    [SerializeField] LayerMask wallMask;
    [Tooltip("What will stop or limit an explosion's path")]
    [SerializeField] string rockTag;
    [Tooltip("REFERENCE to the explsion prefab")]
    [SerializeField] GameObject explosionEffect;
    [Tooltip("REFERENCE to the audioClip for explosions")]
    [SerializeField] AudioClip explosionAudio;
    [Tooltip("REFERENCE to the audio for the fuse")]
    [SerializeField] AudioSource fuseAudio;
    /// <summary>
    /// The logic for spawning all explosions. Using several raycasts the bomb will look for any walls in its way and limit its explosion accordingly
    /// </summary>
    public void Explode()
    {
        //right
        RaycastHit hit;
        Instantiate(explosionEffect, transform.position, transform.rotation);
        fuseAudio.Stop();
        if (Physics.Raycast(transform.position, new Vector3(1, 0f, 0f), out hit, range*2 , wallMask))
        {
            Debug.Log(Mathf.RoundToInt(Vector3.Distance(transform.position, hit.transform.position)/2));
            int x = Mathf.RoundToInt(Vector3.Distance(transform.position, hit.transform.position) / 2);
            if(hit.transform.gameObject.tag==rockTag)
            {
                x += 1;
            }
            for (int i = 0; i <x-1; i++)
            {
                Instantiate(explosionEffect, transform.position + new Vector3(2 + (2.0f * i), 0.0f, 0f), transform.rotation);
            }
        }
        else
        {
            for(int i=0;i<range;i++)
            {
                Instantiate(explosionEffect, transform.position + new Vector3(2+(2.0f*i), 0.0f, 0f), transform.rotation);
            }
        }





        //left
        if (Physics.Raycast(transform.position, new Vector3(-1, 0f, 0f), out hit, range * 2, wallMask))
        {
            Debug.Log(Mathf.RoundToInt(Vector3.Distance(transform.position, hit.transform.position) / 2));
            int x = Mathf.RoundToInt(Vector3.Distance(transform.position, hit.transform.position) / 2);
            if (hit.transform.gameObject.tag == rockTag)
            {
                x += 1;
            }
            for (int i = 0; i < x - 1; i++)
            {
                Instantiate(explosionEffect, transform.position + new Vector3(-2 + (-2.0f * i), 0.0f, 0f), transform.rotation);
            }
        }
        else
        {
            for (int i = 0; i < range; i++)
            {
                Instantiate(explosionEffect, transform.position + new Vector3(-2 + (-2.0f * i), 0.0f, 0f), transform.rotation);
            }
        }
        //up
        if (Physics.Raycast(transform.position + new Vector3(0.5f, 0.0f, 0f), new Vector3(0, 0f, 1f), out hit, range * 2, wallMask))
        {
            Debug.Log(Mathf.RoundToInt(Vector3.Distance(transform.position, hit.transform.position) / 2));
            int x = Mathf.RoundToInt(Vector3.Distance(transform.position, hit.transform.position) / 2);
            if (hit.transform.gameObject.tag == rockTag)
            {
                x += 1;
            }
            for (int i = 0; i < x - 1; i++)
            {
                Instantiate(explosionEffect, transform.position + new Vector3(0, 0.0f, 2 + (2.0f * i)), transform.rotation);
            }
        }
        else
        {
            for (int i = 0; i < range; i++)
            {
                Instantiate(explosionEffect, transform.position + new Vector3(0, 0.0f, 2 + (2.0f * i)), transform.rotation);
            }
        }
        //down
        if (Physics.Raycast(transform.position + new Vector3(0.5f, 0.0f, 0f), new Vector3(0, 0f, -1f), out hit, range * 2, wallMask))
        {
           
            int x = Mathf.RoundToInt(Vector3.Distance(transform.position, hit.transform.position) / 2);
            if (hit.transform.gameObject.tag == rockTag)
            {
                x += 1;
            }
            for (int i = 0; i < x - 1; i++)
            {
                Instantiate(explosionEffect, transform.position + new Vector3(0, 0.0f, -2 + (-2.0f * i)), transform.rotation);
            }
        }
        else
        {
            for (int i = 0; i < range; i++)
            {
                Instantiate(explosionEffect, transform.position + new Vector3(0, 0.0f, -2 + (-2.0f * i)), transform.rotation);
            }
        }
        gameObject.SetActive(false);
        MMSoundManager.Instance.PlaySound(explosionAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
      false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
      1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    private void OnEnable()
    {
        currentDetonationTime = maxDetonationTime;
        fuseAudio.Play();
    }
    private void Update()
    {
        currentDetonationTime -= Time.deltaTime;
        if (currentDetonationTime <= 0)
            Explode();
    }
}
