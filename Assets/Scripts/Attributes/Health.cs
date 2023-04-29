using UnityEngine;
using UnityEngine.Events;
using GameDevTV.Utils;
using GameDevTV.Saving;
using GameDevTV.Inventories;
using RPG.Stats;
using RPG.Core;
using RPG.Inventories;
using System;

namespace RPG.Attributes
{    
    public class Health : MonoBehaviour, ISaveable
    {
        //The Health class manages the health of a GameObject:
        // - Retrieves max health from Basetats;
        // - Manages and tracks changes to health points;
        // - Manages death (including awarding of experience to killer)

        // Events
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent onDie;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        // Cache
        LazyValue<float> healthPoints;
        private bool hasDied = false;

        private BaseStats baseStats;
        private StatsEquipment equipment;

        public event Action onHealthChanged;

        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();
            equipment = GetComponent<StatsEquipment>();

            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return baseStats.GetStat(Stat.Health);
        }

        private void OnEnable()
        {
            baseStats.onLevelUp += RestoreHealthToMax;
            if (equipment)
            {
                equipment.equipmentUpdated += InvokeOnHealthChanged;
            }
        }

        private void OnDisable()
        {
            baseStats.onLevelUp -= RestoreHealthToMax;
            if (equipment)
            {
                equipment.equipmentUpdated -= InvokeOnHealthChanged;
            }
        }

        private void Start()
        {
            //Force initialization of healthPoints if it has not yet been initialized
            healthPoints.ForceInit();

            // Triggers onHealthChanged event to ensure that any class that needs to be updated on health points is up to date
            if (onHealthChanged != null)
            {
                onHealthChanged();
            }
        }

        public bool IsDead()
        {
            return hasDied;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {

            // Subtracts damage from health points but hp cannot go below zero
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);

            // Temporary, for easier debugging
            print($"{gameObject.name} took damage: {damage}");

            // Call events to notify other classes that damage was taken
            takeDamage.Invoke(damage);

            if (onHealthChanged != null)
            {
                onHealthChanged();
            }

            if (healthPoints.value == 0 && hasDied == false)
            {
                onDie.Invoke();
                AwardExperience(instigator);
                Die();
            }
          
        }

        public void Heal (float healthToRestore)
        {
            healthPoints.value += healthToRestore;
            healthPoints.value = Mathf.Clamp(healthPoints.value, 0, GetMaxHealthPoints());
            onHealthChanged.Invoke();
        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return baseStats.GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * GetFraction();
        }

        public float GetFraction()
        {
            return healthPoints.value / baseStats.GetStat(Stat.Health);
        }

        private void RestoreHealthToMax()
        {
            healthPoints.value = baseStats.GetStat(Stat.Health);
            onHealthChanged();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperience(baseStats.GetStat(Stat.ExperienceReward));
        }

        private void InvokeOnHealthChanged()
        {
            onHealthChanged.Invoke();
        }

        private void Die()
        {
            GetComponent<Animator>().SetTrigger("die");
            hasDied = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }


        // ISaveable methods below

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;
            if (healthPoints.value == 0)
            {
                Die();
            }
            if (onHealthChanged != null)
            {
                onHealthChanged();
            }
        }
    }
}

