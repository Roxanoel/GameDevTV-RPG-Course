using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestStatus
    {
        Quest quest;
        List<string> completedObjectives = new List<string>();

        [System.Serializable] class QuestStatusRecord
        {
            public string questName;
            public List<string> completedObjectives;

            public QuestStatusRecord(QuestStatus status)
            {
                this.questName = status.quest.GetTitle();
                this.completedObjectives = status.completedObjectives;
            }
        }

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
        }

        public QuestStatus(object objectState)
        {
            QuestStatusRecord state = objectState as QuestStatusRecord;
            quest = Quest.GetByName(state.questName);
            completedObjectives = state.completedObjectives;
        }

        public Quest GetQuest()
        {
            return quest;
        }

        public int GetCompletedObjectivesCount()
        {
            return completedObjectives.Count;
        }

        public bool IsObjectiveComplete(string objective)
        {
            return completedObjectives.Contains(objective);
        }

        public void CompleteObjective(string objective)
        {
            if (quest.HasObjective(objective))
            {
                completedObjectives.Add(objective);
            }
        }

        public object CaptureState()
        {
            return new QuestStatusRecord(this);
        }

        public bool isComplete()
        {
            return true; // for now
        }
    }
}
