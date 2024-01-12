using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Inventories;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName ="Quest", order = 0)]

    public class Quest : ScriptableObject
    {
        [SerializeField] Objective[] objectives;
        [SerializeField] List<Reward> rewards = new List<Reward>();

        [System.Serializable]
        class Reward
        {
            public int number;
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
            return objectives.Length;
        }

        public Objective[] GetObjectives()
        {
            return objectives;
        }

        public string GetTitle()
        {
            return this.name;
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
