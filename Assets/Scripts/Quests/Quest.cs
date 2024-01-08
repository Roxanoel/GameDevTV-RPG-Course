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
        
    }
}
