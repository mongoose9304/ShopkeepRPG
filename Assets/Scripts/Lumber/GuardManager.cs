using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Noise
{
    public Transform location;
    public int levelOfNoise;
}
public class GuardManager : MonoBehaviour
{
    public static GuardManager instance;
    protected Noise loudestNoise;
    [SerializeField] List<BasicGuard> myGuards = new List<BasicGuard>();

    private void Awake()
    {
        instance = this;
    }
    public void CreateNoise(Transform loc_,int levelOfNoise_)
    {
        if(levelOfNoise_>=loudestNoise.levelOfNoise)
        {
            loudestNoise.location = loc_;
            loudestNoise.levelOfNoise = levelOfNoise_;
            AlertGuards();
        }


    }
    public void AlertGuards()
    {
        foreach(BasicGuard guard in myGuards)
        {
            guard.SetSearchTarget(loudestNoise.location);
        }
    }
    public void LoudestNoiseInvestigated()
    {
        loudestNoise.levelOfNoise = -1;
    }
    public void PlayerAttemptingHide(GameObject player)
    {
        foreach (BasicGuard guard in myGuards)
        {
            guard.AttemptHide(player);
        }
    }


}
