using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GameDevTV.Saving;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour, ISaveable
    {
        List<QuestStatus> statuses = new List<QuestStatus>();
        public event Action onListUpdated;

        public IEnumerable<QuestStatus> GetStatuses()
        {
            return statuses;
        }

        private QuestStatus GetQuestStatus(Quest quest)
        {
            foreach (QuestStatus status in statuses)
            {
                if (status.GetQuest() == quest)
                {
                    return status;
                }
            }
            return null;
        }

        public bool HasQuest(Quest quest)
        {
            return GetQuestStatus(quest) != null;
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

        public void CompleteObjective(Quest quest, string objectiveRef)
        {
            QuestStatus status = GetQuestStatus(quest);
            status.CompleteObjective(objectiveRef);
            if (onListUpdated != null)
            {
                onListUpdated();
            }
        }

        public object CaptureState()
        {
            List<object> state = new List<object>();

            foreach (QuestStatus status in statuses)
            {
                state.Add(status.CaptureState());
            }

            return state;
        }

        public void RestoreState(object state)
        {
            List<object> stateList = state as List<object>;
            if (stateList == null) return;

            // Clear any state that might remain
            statuses.Clear();
            // Repopulate list from saved state
            foreach (object objectState in stateList)
            {
                statuses.Add(new QuestStatus(objectState));
            }
        }
    }
}
