using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;


public class EnemyManager : MonoBehaviour
{

    public GameObject EnemyItemPrefab;
    public List<EnemyItem> enemies = new List<EnemyItem>();
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
    public void CreateEnemyItem(string name_,GameObject object_)
    {
        foreach(EnemyItem item_ in enemies)
        {
            if (item_.name == name_)
                return;
        }
        GameObject obj = Instantiate(EnemyItemPrefab);
        obj.transform.SetParent(this.transform);
        obj.GetComponent<EnemyItem>().myname = name_;
        obj.GetComponent<EnemyItem>().pooler.GameObjectToPool = object_;
        obj.GetComponent<EnemyItem>().pooler.FillObjectPool();
        enemies.Add(obj.GetComponent<EnemyItem>());
    }

    public void SpawnEnemy(string name_,Transform pos_,EnemyCounter counter_=null)
    {
        foreach(EnemyItem item_ in enemies)
        {
          
            if(item_.myname==name_)
            {
             
             GameObject obj=item_.pooler.GetPooledGameObject();
                obj.transform.position = pos_.position;
                obj.SetActive(true);
                currentEnemiesList.Add(obj);
                if (counter_) obj.GetComponent<BasicEnemy>().myEnemyCounter = counter_;
            }
        }
    }
    private void CleanCurrentEnemyList()
    {
       // if(currentEnemiesList.Count>0)
       // currentEnemiesList.RemoveAll(null);

    }

}
