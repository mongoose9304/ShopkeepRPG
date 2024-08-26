using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;


public class EnemyManager : MonoBehaviour
{

    public GameObject EnemyItemPrefab;
    public CombatPlayerMovement playerMovement;
    public List<EnemyItem> enemies = new List<EnemyItem>();
    public List<EnemyItem> eliteEnemies = new List<EnemyItem>();
    public static EnemyManager instance;
    public List<GameObject> currentEnemiesList = new List<GameObject>();
    public float timeBetweenEnemyListCleans;
    float currentTimeBetweenEnemyListCleans;

    //hitParticles
    [SerializeField] protected MMMiniObjectPooler physicalHitEffects;
    [SerializeField] protected MMMiniObjectPooler airHitEffects;
    [SerializeField] protected MMMiniObjectPooler fireHitEffects;
    [SerializeField] protected MMMiniObjectPooler waterHitEffects;

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

    public void SpawnEnemy(string name_,Transform pos_,EnemyCounter counter_=null,int enemyLevel=1)
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
                obj.SetActive(true);
                currentEnemiesList.Add(obj);
                if (counter_) obj.GetComponent<BasicEnemy>().myEnemyCounter = counter_;
                break;
            }
        }
    }
    public void SpawnEliteEnemy(string name_, Transform pos_, EnemyCounter counter_ = null, int enemyLevel = 1)
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
                obj.SetActive(true);
                currentEnemiesList.Add(obj);
                if (counter_) obj.GetComponent<BasicEnemy>().myEnemyCounter = counter_;
                break;
            }
        }
    }
    public void SpawnRandomEnemy(bool isElite_, Transform pos_, EnemyCounter counter_ = null, int enemyLevel = 1)
    {
        string name = "";
        if (!isElite_)
        {
             name = enemies[Random.Range(0, enemies.Count)].myname;
            SpawnEnemy(name, pos_, counter_,enemyLevel);
        }
        else
        {
            name = eliteEnemies[Random.Range(0, eliteEnemies.Count)].myname;
            SpawnEliteEnemy(name, pos_, counter_, enemyLevel);
        }
    }
    private void CleanCurrentEnemyList()
    {
        for(int i=0;i<currentEnemiesList.Count;i++)
        {
            if (!currentEnemiesList[i].activeInHierarchy)
                currentEnemiesList.RemoveAt(i);
        }
       

    }
    public void DisableAllEnemies()
    {
        for (int i = 0; i < currentEnemiesList.Count; i++)
        {
            currentEnemiesList[i].SetActive(false);
        }
    }
    public void EnemyDeath()
    {
        playerMovement.GetAKill();
    }
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
            default:
                return;
                
        }
        obj.transform.position = location_.position;
        obj.transform.rotation = location_.rotation;
        obj.SetActive(true);
    }

}
