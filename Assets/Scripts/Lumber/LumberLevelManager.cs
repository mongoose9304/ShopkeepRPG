using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine.SceneManagement;

public class LumberLevelManager : MonoBehaviour
{
    public static LumberLevelManager instance;
    public LumberLevel currentLevel;
    public LumberLevel nextLevel;
    public LumberPlayer[] players;
    //ranges from 0.5 to 1.5, the rough amount of how much of each reasource your should be aquiring. ALso affects visuals 
    public float forestHealth;
    public float currentForestHealthChange;
    [Tooltip("REFERNCE to the pool of wood world objects")]
    [SerializeField] MMMiniObjectPooler woodWorldObjectPooler;
    [Tooltip("REFERNCE to the pool of multiwood world objects")]
    [SerializeField] MMMiniObjectPooler multiwoodWorldObjectPooler;
    [Tooltip("REFERENCE to BGMs available")]
    public List<AudioClip> BGMs = new List<AudioClip>();
    public GameObject victoryLevel;
    [Tooltip("Sprites for collected resources ")]
    public List<Sprite> resourceSprites = new List<Sprite>();
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Using pools spawn a lumber object at target location with a set value 
    /// </summary>
    public void SpawnLumber(Transform location_, int value_ = 1, bool useMultiWood = false)
    {
        GameObject obj = null;
        if (useMultiWood)
        {
             obj = multiwoodWorldObjectPooler.GetPooledGameObject();
        }
        else
        {
             obj = woodWorldObjectPooler.GetPooledGameObject();
        }
       
        obj.GetComponent<LumberPickUp>().lumberAmount = value_;
        
        obj.transform.position = location_.position;
        obj.transform.rotation = location_.rotation;
        obj.SetActive(true);
    }
    private void Start()
    {
        PlayRandomBGM();
    }
    public void PlayNextLevel()
    {
        forestHealth = PlayerPrefs.GetFloat("ForestHealth", 1);
        SetUpCurrentLevel();
        currentForestHealthChange = 0;
    }
    private void SetUpCurrentLevel()
    {
        currentLevel.SpawnLootables(forestHealth);
        currentLevel.SpawnAllPuzzles(forestHealth);
        currentLevel.SetUpDecorations(forestHealth);
        foreach (LumberPlayer playa in players)
        {
            playa.transform.position = currentLevel.playerStart.position;
        }
    }
    public void TreeFall()
    {
        currentForestHealthChange += 0.01f;
    }
    public void TreeReplanted()
    {
        currentForestHealthChange -= 0.01f;
    }
    public void EndLevel()
    {
        if (currentForestHealthChange < 0)
            currentForestHealthChange = 0;
        if(currentForestHealthChange>10)
            currentForestHealthChange = 10;
        forestHealth -= currentForestHealthChange;
    }
    public void PlayRandomBGM()
    {
        MMSoundManager.Instance.StopTrack(MMSoundManager.MMSoundManagerTracks.Music);
        MMSoundManager.Instance.PlaySound(BGMs[Random.Range(0, BGMs.Count)], MMSoundManager.MMSoundManagerTracks.Music, Vector3.zero, true,0.75f);
    }
    public void DebugSetForestHealth(float newHP)
    {
        PlayerPrefs.SetFloat("ForestHealth",newHP);
        forestHealth = newHP;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    /// <summary>
    /// Stop playing lumber, display your score then return to the overworld
    /// </summary>
    public void WinLevel(bool hasLost_ = false)
    {
        
        victoryLevel.SetActive(true);
        if (LootManager.instance.AquiredLootItems.Count >= 1)
            LootDisplayManager.instance.AddItems(LootManager.instance.currentLootItems);
        List<int> x = new List<int>();
        //here is where you would load the inventorys count of how many resources *Regular cash* you have.
        List<int> y = new List<int>();
        y.Add(500);
        //y.Add(1000);
        x.Add(LootManager.instance.currentResource);
       // x.Add(LootManager.instance.regularCurrentCash);
        LootDisplayManager.instance.AddResources(x, y, resourceSprites);
        LootDisplayManager.instance.StartVictoryScreen(hasLost_);
        PlayerManager.instance.TemporaryDisablePlayer2();
    }
}
