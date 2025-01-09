using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamiliarSwitcher : MonoBehaviour
{
    private int currentFam;
    public GameObject[] Familiars;
    public SceneSpecificPlayerManager sceneSpecificplayerManager;
    private void OnEnable()
    {
        foreach (GameObject obj in Familiars)
        {
            obj.gameObject.SetActive(false);
        }
        if (PlayerManager.instance)
        {
            switch (PlayerManager.instance.currentFamiliar)
            {
                case Familiar.Slime:
                    Familiars[0].SetActive(true);
                    currentFam = 0;
                    break;
                case Familiar.Skeleton:
                    Familiars[1].SetActive(true);
                    currentFam = 1;
                    break;

            }
        }
    }
    public void CycleFamiliars()
    {
        currentFam += 1;
        if(currentFam>=Familiars.Length)
        {
            currentFam = 0;
        }
        switch (currentFam)
        {
            case 0:
                SwapFamiliar(Familiar.Slime);
                break;
            case 1:
                SwapFamiliar(Familiar.Skeleton);
                break;

        }
    }
    public void SwapFamiliar(Familiar fam_)
    {
        PlayerManager.instance.SwitchFamiliar(fam_);
        foreach (GameObject obj in Familiars)
        {
            obj.gameObject.SetActive(false);
        }
        if (PlayerManager.instance)
        {
            switch (PlayerManager.instance.currentFamiliar)
            {
                case Familiar.Slime:
                    Familiars[0].SetActive(true);
                    currentFam = 0;
                    break;
                case Familiar.Skeleton:
                    Familiars[1].SetActive(true);
                    currentFam = 1;
                    break;

            }
        }
        if(sceneSpecificplayerManager)
        {
            sceneSpecificplayerManager.SwitchFamiliar();
        }
    }
}
