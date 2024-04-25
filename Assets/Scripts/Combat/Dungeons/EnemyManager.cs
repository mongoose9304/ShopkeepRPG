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

    public void SpawnEnemy(string name_,Transform pos_)
    {
        foreach(EnemyItem item_ in enemies)
        {
            Debug.Log("Item_ name " +item_.myname);
            Debug.Log("IName name " +name_);
            if(item_.myname==name_)
            {
                Debug.Log("name MAtch");
             GameObject obj=item_.pooler.GetPooledGameObject();
                obj.transform.position = pos_.position;
                obj.SetActive(true);
                currentEnemiesList.Add(obj);
            }
        }
    }
    private void CleanCurrentEnemyList()
    {
       // if(currentEnemiesList.Count>0)
       // currentEnemiesList.RemoveAll(null);

    }

}
