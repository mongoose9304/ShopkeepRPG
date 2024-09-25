using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
public class SceneAudioManager : MonoBehaviour
{
    public List<AudioClip> BGMs = new List<AudioClip>();

    private void Update()
    {
        if (!MMSoundManager.Instance.HasSoundsPlaying(MMSoundManager.MMSoundManagerTracks.Music))
        {
            PlayRandomBGM();
        }
    }
    public void PlayRandomBGM()
    {
        MMSoundManager.Instance.PlaySound(BGMs[Random.Range(0,BGMs.Count)], MMSoundManager.MMSoundManagerTracks.Music, Vector3.zero);
    }
}
