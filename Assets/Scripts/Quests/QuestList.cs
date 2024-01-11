using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour
    {
        List<QuestStatus> statuses = new List<QuestStatus>();
        public event Action onListUpdated;

        public IEnumerable<QuestStatus> GetStatuses()
        {
            return statuses;
        }

        public bool HasQuest(Quest quest)
        {
            foreach (QuestStatus status in statuses)
            {
                if (status.GetQuest() == quest) return true;
            }
            
            return false;
        }

        public void AddQuest(Quest quest)
        {
            // Check if quest is already there, if so early return
            if (HasQuest(quest)) return;
            // Create quest status and add it to list
            QuestStatus newStatus = new QuestStatus(quest);
            statuses.Add(newStatus);
            // Event
            if (onListUpdated != null)
            {
                onListUpdated();
            }
        }
    }
}
