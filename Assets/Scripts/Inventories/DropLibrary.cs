using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Inventories;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = ("Inventory/Drop Library"))]
    public class DropLibrary : ScriptableObject
    {
        // - Drop chance
        // - Min drops
        // - Max drops
        // - Potential drops
        //   - Relative chance
        //   - Min items
        //   - Max items

        [Header("General config, by level")]
        [SerializeField] float[] dropChancePercentage;
        [SerializeField] int[] minDrops;
        [SerializeField] int[] maxDrops;
        [Header("Item drop configs")]
        [SerializeField] DropConfig[] potentialDrops;

        [System.Serializable] 
        class DropConfig
        {
            public InventoryItem item;
            public float[] relativeChance;
            public int[] minNumber;
            public int[] maxNumber;
            public int GetRandomNumber(int level)
            {
                if (!item.IsStackable())
                {
                    return 1;
                }
                int min = GetByLevel(minNumber, level);
                int max = GetByLevel(maxNumber, level);
                return Random.Range(min, max + 1);
            }
        }

        public struct Dropped
        {
            public InventoryItem item;
            public int number;
            public Dropped(InventoryItem item, int number)
            {
                this.item = item;
                this.number = number;
            }
        }

        public IEnumerable<Dropped> GetRandomDrops(int level)
        {
            // Checks if there should be a random drop at all
            if (!ShouldRandomDrop(level))
            {
                yield break;
            }
            // If so, get a random number of drops from the allowed range
            for (int i = 0; i< GetRandomNumberOfDrops(level); i++)
            {
                yield return GetRandomDrop(level);
            }
        }

        private bool ShouldRandomDrop(int level)
        {
            float randomRoll = Random.Range(0f, 100f);
            return randomRoll < GetByLevel(dropChancePercentage, level);       
        }

        private int GetRandomNumberOfDrops(int level)
        {
            return Random.Range(GetByLevel(minDrops, level), GetByLevel(maxDrops, level) + 1);
        }

        private Dropped GetRandomDrop(int level)
        {
            var drop = SelectRandomItem(level);
            Dropped dropped = new Dropped(drop.item, drop.GetRandomNumber(level));
            return dropped;

        }

        private DropConfig SelectRandomItem(int level)
        {
            float totalChance = GetTotalChance(level) ;
            float randomRoll = Random.Range(0, totalChance);
            float chanceTotal = 0;
            foreach (var drop in potentialDrops)
            {
                chanceTotal += GetByLevel(drop.relativeChance, level);
                if (chanceTotal > randomRoll)
                {
                    return drop;
                }
            }
            return null;
        }

        private float GetTotalChance(int level)
        {
            float totalChance = 0;
            foreach (DropConfig drop in potentialDrops)
            {
                totalChance += GetByLevel(drop.relativeChance, level);
            }
            return totalChance;
        }

        static T GetByLevel<T>(T[] values, int level)
        {
            if (values.Length == 0)
            {
                return default;

            }
            if (level > values.Length)
            {
                return values[values.Length - 1];
            }
            if (level < 0)
            {
                return default;
            }
            return values[level - 1];
        }
    }
}
