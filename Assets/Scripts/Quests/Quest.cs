using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Inventories;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName ="Quest", order = 0)]

    public class Quest : ScriptableObject
    {
        [SerializeField] List<Objective> objectives = new List<Objective>();
        [SerializeField] List<Reward> rewards = new List<Reward>();

        [System.Serializable]
        public class Reward
        {
            [Min(1)]public int number;
            public InventoryItem item;
        }

        [System.Serializable]
        public class Objective
        {
            public string reference;
            public string description;

            public Objective(string reference, string description)
            {
                this.reference = reference;
                this.description = description;
            }
        }

        public int GetObjectivesCount()
        {
            return objectives.Count;
        }

        public bool HasObjective(string objectiveRef)
        {
            foreach (Objective objective in objectives)
            {
                if (objective.reference == objectiveRef) return true;
            }
            return false;
        }

        public IEnumerable<Objective> GetObjectives()
        {
            return objectives;
        }

        public string GetTitle()
        {
            return this.name;
        }

        public IEnumerable<Reward> GetRewards()
        {
            return rewards;
        }

        public static Quest GetByName(string questName)
        {
            foreach (Quest quest in Resources.LoadAll<Quest>(""))
            {
                if (quest.name == questName) return quest;
            }
            // If no match is found
            return null;
        }
        
    }
}
