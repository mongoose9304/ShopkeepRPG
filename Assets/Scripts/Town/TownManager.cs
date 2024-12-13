using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TownManager : MonoBehaviour
{
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
}
