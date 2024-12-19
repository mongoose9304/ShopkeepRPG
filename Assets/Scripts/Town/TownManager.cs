using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TownManager : MonoBehaviour
{
    public List<AudioClip> BGMs = new List<AudioClip>();
    private void Start()
    {
        PlayRandomBGM();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void ResetAllSavedData()
    {
        FileHandler.DeleteFile("MoveableObjectInventory");
        FileHandler.DeleteFile("PlayerMoveableInventory");
        FileHandler.DeleteFile("PlayerInventory");
        FileHandler.DeleteFile("BarginBinInventory");
        FileHandler.DeleteFile("BarginBinInventoryPrevious");
        FileHandler.DeleteFile("BarginBinDiscounts");

        FileHandler.DeleteFile("PedestalInventory");
        FileHandler.DeleteFile("PedestalInventoryPrevious");
        PlayerPrefs.DeleteAll();
        string currentSceneName = SceneManager.GetActiveScene().name;
        TempPause.instance.TogglePause();
        SceneManager.LoadScene(currentSceneName);
    }
    public void PlayRandomBGM()
    {
        MMSoundManager.Instance.StopTrack(MMSoundManager.MMSoundManagerTracks.Music);
        MMSoundManager.Instance.PlaySound(BGMs[Random.Range(0, BGMs.Count)], MMSoundManager.MMSoundManagerTracks.Music, Vector3.zero, true, 0.6f);
    }
}
