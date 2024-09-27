using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

/// <summary>
/// A singleton class that manages the combat enemies and spawns 
/// </summary>
public class EnemyManager : MonoBehaviour
{
    [Tooltip("List of all the enemy types")]
    public List<EnemyItem> enemies = new List<EnemyItem>();
    [Tooltip("List of all the elite types")]
    public List<EnemyItem> eliteEnemies = new List<EnemyItem>();
    [Tooltip("Singleton instance")]
    public static EnemyManager instance;
    [Tooltip("The current active enemies for lock ons and such")]
    public List<GameObject> currentEnemiesList = new List<GameObject>();
    [Tooltip("We shouldnt clean the list every frame but it does need to happen")]
    public float timeBetweenEnemyListCleans;
    float currentTimeBetweenEnemyListCleans;
    [Tooltip("The teams we can spawn, set by the current Dungeon")]
    [SerializeField] public List<string> randomTeams = new List<string>();
    [Tooltip("Should we be using multiple teams?")]
    [SerializeField] public bool useRandomTeams;
    [Tooltip("The team tag if theres only one, usuallyw ahtever sin domain we are in")]
    [SerializeField] public string singleTeam;
    //hitParticles
    [Header("References")]
    [Tooltip("Reference to the Enemy Item Prefab")]
    public GameObject EnemyItemPrefab;
    [Tooltip("Reference player script")]
    public CombatPlayerMovement playerMovement;
    [Tooltip("Reference particles that can play when hitting an enemy")]
    [SerializeField] protected MMMiniObjectPooler physicalHitEffects;
    [Tooltip("Reference particles that can play when hitting an enemy")]
    [SerializeField] protected MMMiniObjectPooler airHitEffects;
    [Tooltip("Reference particles that can play when hitting an enemy")]
    [SerializeField] protected MMMiniObjectPooler fireHitEffects;
    [Tooltip("Reference particles that can play when hitting an enemy")]
    [SerializeField] protected MMMiniObjectPooler waterHitEffects;
    [Tooltip("Reference particles that can play when hitting an enemy")]
    [SerializeField] protected MMMiniObjectPooler earthHitEffects;
    [Tooltip("Reference effects that play when killing an enemy")]
    [SerializeField] protected MMMiniObjectPooler[] cartoonDeathEffects;
    [Tooltip("Reference audios that play when killing an enemy")]
    [SerializeField] protected AudioClip[] enemyDeathAudios;

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        currentTimeBetweenEnemyListCleans -= Time.deltaTime;
        if (currentTimeBetweenEnemyListCleans <= 0)
        {
            currentTimeBetweenEnemyListCleans = timeBetweenEnemyListCleans;
            CleanCurrentEnemyList();
        }
    }
    /// <summary>
    /// Create an enemy item so we can pool this enemy
    /// </summary>
    public void CreateEnemyItem(string name_,GameObject object_,bool isElite=false)
    {
        if(!isElite)
        { 
            foreach(EnemyItem item_ in enemies)
            {
               if (item_.name == name_)
                return;
            }
        }
        else
        {
            foreach (EnemyItem item_ in eliteEnemies)
            {
                if (item_.name == name_)
                    return;
            }
        }
        GameObject obj = Instantiate(EnemyItemPrefab);
        obj.transform.SetParent(this.transform);
        obj.GetComponent<EnemyItem>().myname = name_;
        obj.GetComponent<EnemyItem>().pooler.GameObjectToPool = object_;
        obj.GetComponent<EnemyItem>().pooler.FillObjectPool();
        if(!isElite)
            enemies.Add(obj.GetComponent<EnemyItem>());
        else
            eliteEnemies.Add(obj.GetComponent<EnemyItem>());
    }
    /// <summary>
    /// Create an enemy at a location
    /// </summary>
    public void SpawnEnemy(string name_,Transform pos_,EnemyCounter counter_=null,int enemyLevel=1,string team="")
    {
        foreach(EnemyItem item_ in enemies)
        {
          
            if(item_.myname==name_)
            {
             
             GameObject obj=item_.pooler.GetPooledGameObject();
                obj.transform.position = pos_.position;
                if(obj.TryGetComponent<BasicEnemy>(out BasicEnemy enemy))
                {
                    enemy.Level = enemyLevel;
                }
               
                currentEnemiesList.Add(obj);
                if (counter_) obj.GetComponent<BasicEnemy>().myEnemyCounter = counter_;
                if(obj.TryGetComponent<TeamUser>(out TeamUser t))
                {
                    if (team == "")
                    {
                        if (useRandomTeams)
                        {
                            t.myTeam = randomTeams[Random.Range(0, randomTeams.Count)];
                        }
                        else
                        {
                            t.myTeam = singleTeam;
                        }
                    }
                    else
                    {
                        t.myTeam = team;
                    }
                }
                obj.SetActive(true);
                break;
            }
        }
    }
    /// <summary>
    /// Create an elite enemy at a location
    /// </summary>
    public void SpawnEliteEnemy(string name_, Transform pos_, EnemyCounter counter_ = null, int enemyLevel = 1,string team="")
    {
        foreach (EnemyItem item_ in eliteEnemies)
        {

            if (item_.myname == name_)
            {

                GameObject obj = item_.pooler.GetPooledGameObject();
                obj.transform.position = pos_.position;
                if (obj.TryGetComponent<BasicEnemy>(out BasicEnemy enemy))
                {
                    enemy.Level = enemyLevel;
                }
               
                currentEnemiesList.Add(obj);
                if (counter_) obj.GetComponent<BasicEnemy>().myEnemyCounter = counter_;
                if (obj.TryGetComponent<TeamUser>(out TeamUser t))
                {
                    if (team == "")
                    {
                        if (useRandomTeams)
                        {
                            t.myTeam = randomTeams[Random.Range(0, randomTeams.Count)];
                        }
                        else
                        {
                            t.myTeam = singleTeam;
                        }
                    }
                    else
                    {
                        t.myTeam = team;
                    }
                }
                obj.SetActive(true);
                break;
            }
        }
    }
    /// <summary>
    /// Spawn a random enemy at a location. The enemy will be randomized from the ones available in this dungeon
    /// </summary>
    public void SpawnRandomEnemy(bool isElite_, Transform pos_, EnemyCounter counter_ = null, int enemyLevel = 1,string team="")
    {
        string name = "";
        if (!isElite_)
        {
             name = enemies[Random.Range(0, enemies.Count)].myname;
            SpawnEnemy(name, pos_, counter_,enemyLevel,team);
        }
        else
        {
            name = eliteEnemies[Random.Range(0, eliteEnemies.Count)].myname;
            SpawnEliteEnemy(name, pos_, counter_, enemyLevel,team);
        }
    }
    /// <summary>
    /// Remove any unused enemies
    /// </summary>
    private void CleanCurrentEnemyList()
    {
        for(int i=0;i<currentEnemiesList.Count;i++)
        {
            if (!currentEnemiesList[i].activeInHierarchy)
                currentEnemiesList.RemoveAt(i);
        }
       

    }
    /// <summary>
    /// Disables all active enemies in the scene
    /// </summary>
    public void DisableAllEnemies()
    {
        for (int i = 0; i < currentEnemiesList.Count; i++)
        {
            currentEnemiesList[i].SetActive(false);
        }
    }
    /// <summary>
    /// An enemy has died, inform the player for any on Kill abilities 
    /// </summary>
    public void EnemyDeath(Vector3 pos_)
    {
        playerMovement.GetAKill();
        GameObject obj=cartoonDeathEffects[Random.Range(0,cartoonDeathEffects.Length)].GetPooledGameObject();
        obj.transform.position = pos_;
        obj.SetActive(true);
        MMSoundManager.Instance.PlaySound(enemyDeathAudios[Random.Range(0, enemyDeathAudios.Length)], MMSoundManager.MMSoundManagerTracks.Sfx, pos_,
     false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.9f, 1.1f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
     1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    /// <summary>
    /// Create an elemental effect at the location we have hit an enemy
    /// </summary>
    public void ApplyHitEffect(Element element_, Transform location_)
    {
        GameObject obj;
        switch(element_)
        {
            case Element.Neutral:
                obj = physicalHitEffects.GetPooledGameObject();
                break;
            case Element.Air:
                obj = airHitEffects.GetPooledGameObject();
                break;
            case Element.Fire:
                obj = fireHitEffects.GetPooledGameObject();
                break;
            case Element.Water:
                obj = waterHitEffects.GetPooledGameObject();
                break;
            case Element.Earth:
                obj = earthHitEffects.GetPooledGameObject();
                break;
            default:
                return;
                
        }
        obj.transform.position = location_.position;
        obj.transform.rotation = location_.rotation;
        obj.SetActive(true);
    }
    /// <summary>
    /// Find the nearest target for enemies. This is team based so Greed enemies will try to find Gluttony enemies and so on.
    /// </summary>
    public GameObject FindEnemyTarget(string team_,Vector3 position)
    {
        if(currentEnemiesList.Count==0)
        return null;
        float minDistance=100000;
        GameObject closestEnemy=null;
        for(int i=0;i<currentEnemiesList.Count;i++)
        {
            if(currentEnemiesList[i].gameObject.TryGetComponent<TeamUser>(out TeamUser t_))
            {
                if (t_.myTeam == team_)
                {
                    continue;
                }
            }
            else
            {
                continue;
            }
            if(Vector3.Distance(position,currentEnemiesList[i].transform.position)<minDistance)
            {
                closestEnemy = currentEnemiesList[i];
                minDistance = Vector3.Distance(position, currentEnemiesList[i].transform.position);
            }
        }

        return closestEnemy;
    }
    public void AddEnemyToEnemyList(GameObject enemy_)
    {
        currentEnemiesList.Add(enemy_);
    }

}
