using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName ="Quest", order = 0)]

    public class Quest : ScriptableObject
    {
        [SerializeField] string[] objectives;

        public int GetObjectivesCount()
        {
            return objectives.Length;
        }

        public string[] GetObjectivesList()
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
