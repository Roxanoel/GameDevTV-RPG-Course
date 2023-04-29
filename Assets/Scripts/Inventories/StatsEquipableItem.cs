using GameDevTV.Inventories;
using UnityEngine;
using RPG.Stats;
using System.Collections.Generic;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = ("Inventory/Equipable Item"))]
    public class StatsEquipableItem : EquipableItem, IModifierProvider
    {
        [SerializeField] Modifier[] additiveModifiers;
        [SerializeField] Modifier[] percentageModifiers;

        [System.Serializable]
        struct Modifier
        {
            public Stat stat;
            public float value;
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            float totalAdditiveModifiers = 0;

            foreach (Modifier modifier in additiveModifiers)
            {
                if (modifier.stat == stat)
                {
                    totalAdditiveModifiers += modifier.value;
                }
            }

            yield return totalAdditiveModifiers;
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            float totalPercentageModifiers = 0;

            foreach (Modifier modifier in percentageModifiers)
            {
                if (modifier.stat == stat)
                {
                    totalPercentageModifiers += modifier.value;
                }
            }
            yield return totalPercentageModifiers;
        }
    }
}
