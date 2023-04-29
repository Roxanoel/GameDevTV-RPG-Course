using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GameDevTV.Inventories;
using RPG.Stats;

namespace RPG.Inventories
{
    public class RandomDropper : ItemDropper
    {
        //Config
        [Tooltip("How far can the pickups spawn from the dropper.")]
        [SerializeField] float scatterDistance = 1;
        [Header("Enemy drops config")]
        [SerializeField] DropLibrary dropLibrary;
        [SerializeField] bool isRandomizedDrop;

        //Constants
        const int ATTEMPTS = 30;
        
        public void RandomDrop()
        {
            var baseStats = GetComponent<BaseStats>();
            if (isRandomizedDrop)          
            {               
                var drops = dropLibrary.GetRandomDrops(baseStats.GetLevel());
                foreach (var drop in drops)
                {
                    DropItem(drop.item, drop.number);
                }

            }
        }

        protected override Vector3 GetDropLocation()
        {
            // We might need to attempt it many times
            for (int i = 0; i < ATTEMPTS; i++)
            {
                Vector3 randomPoint = transform.position + (Random.insideUnitSphere * scatterDistance);

                // Make sure the random point is on the navmesh
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                {
                    Debug.Log($"Position was generated on attempt #{i}");
                    return hit.position;
                }
            }
            return transform.position;
        }
    }
}