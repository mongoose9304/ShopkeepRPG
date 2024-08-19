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
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        CleanCurrentEnemyList();
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
       // if(currentEnemiesList.Count>0)
       // currentEnemiesList.RemoveAll(null);

    }
    public void EnemyDeath()
    {
        playerMovement.GetAKill();
    }

}
