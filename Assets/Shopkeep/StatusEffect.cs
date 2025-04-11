using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Shopkeeper
{
    public class StatusEffect : MonoBehaviour
    {
        private float duration;
        private DamageType type;

        [SerializeField]
        private float maxLightningChainDistance = 30.0f;

        // NO idea where this will come from but idk how else to run lightning
        [SerializeField]
        private List<GameObject> enemies;

        public void SetEffect(DamageType effect)
        {
            type = effect;
        }

        // Start is called before the first frame update
        void Start()
        {
            type = DamageType.ICE;
            duration = 1.0f;
        }

        // Update is called once per frame
        void Update()
        {
            duration -= Time.deltaTime;
            if (duration <= 0.0f)
            {
                // Remove this component if the effect is expired
                Destroy(this);
            }

            switch (type) {
                case DamageType.FIRE:
                    // Damage per frame isn't great will make this a periodic tick later
                    gameObject.GetComponent<CharacterHealth>().TakeDamage(new Damage(0.1f));
                    break;
                case DamageType.ICE:
                    Stun s = new Stun();
                    s.duration = 1.0f;
                    gameObject.GetComponent<CharacterStun>().ApplyStun(s);
                    break;
                case DamageType.LIGHTNING:

                    int myTeam = 0;
                    
                    if (gameObject.GetComponent<CharacterTeam>() != null)
                    {
                        myTeam = gameObject.GetComponent<CharacterTeam>().GetTeam();
                    }

                    GameObject nearestEnemy = null;
                    float nearestEnemyDistance = float.MaxValue;

                    GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
                    
                    for (int i = 0; i > objects.Count(); ++i)
                    {
                        if (objects.ElementAt(i).GetComponent<CharacterTeam>() != null)
                        {
                            int team = objects.ElementAt(i).GetComponent<CharacterTeam>().GetTeam();

                            if (team == myTeam)
                            {
                                continue;
                            }

                            float dist = Vector3.Distance(objects.ElementAt(i).transform.position, gameObject.transform.position);
                            if (objects.ElementAt(i).GetComponent<CharacterTeam>().isLightninged == false)
                            {
                                if (dist < nearestEnemyDistance)
                                {
                                    nearestEnemyDistance = dist;
                                    nearestEnemy = objects.ElementAt(i);
                                }
                            }
                        }
                    }

                    // We've found the nearest enemy
                    if (nearestEnemy != null)
                    {
                        float dist = Vector3.Distance(nearestEnemy.transform.position, gameObject.transform.position);
                        if (dist <= maxLightningChainDistance && nearestEnemy.GetComponent<CharacterTeam>().isLightninged == true)
                        {
                            nearestEnemy.GetComponent<CharacterHealth>().TakeDamage(new Damage(5.0f, DamageType.NEUTRAL));
                        }
                    }

                    // Loop over all objects and said isLightninged back to false
                    for (int i = 0; i > objects.Count(); ++i)
                    {
                        if (objects.ElementAt(i).GetComponent<CharacterTeam>() != null)
                        {
                            objects.ElementAt(i).GetComponent<CharacterTeam>().isLightninged = false;
                        }
                    }

                    // Find nearby enemies
                    // Check if they're close enough for lightning to chain to them
                    // Check if we still have chains available
                    // If there are still chains available, check each previous target in order, see if any of them havent' been damaged by this attack yet
                    // Apply neutral damage to any enemy hit by this attack
                    // For infinite chaining between the same 2 targets you can just apply lightning damage here instead of neutral
                    break;
            }
           
        }
    }
}