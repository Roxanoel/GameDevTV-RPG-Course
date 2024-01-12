using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Quests;
using UnityEngine;
using TMPro;

namespace RPG.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Transform objectivesContainer;
        [SerializeField] GameObject objectivePrefab;
        [SerializeField] TextMeshProUGUI rewardsText;
        
        public void Setup(QuestStatus status)
        {
            Quest quest = status.GetQuest();
            // Title
            title.text = quest.GetTitle();
            // Before populating the list of objectives make sure it's empty
            foreach (Transform item in objectivesContainer)
            {
                Destroy(item.gameObject);
            }
            // Objectives 
            foreach (Quest.Objective objective in quest.GetObjectives())
            {
                GameObject objectiveInstance = Instantiate(objectivePrefab, objectivesContainer) as GameObject;
                // Setup the objective
                QuestObjectiveUI objectiveUI = objectiveInstance.GetComponent<QuestObjectiveUI>();
                objectiveUI.SetTitleText(objective.description);
                objectiveUI.SetCompletionStatus(status.IsObjectiveComplete(objective.reference));
            }
            // Rewards
            rewardsText.text = GetRewardsText(quest);
        }

        private string GetRewardsText(Quest quest)
        {
            string rewards = "";
            foreach (Quest.Reward reward in quest.GetRewards())
            {
                if (rewards != "")
                {
                    rewards += ", ";
                }
                if (reward.number > 1)
                {
                    rewards += $"{reward.number}x ";
                }
                rewards += reward.item.GetDisplayName();
            }
            if (rewards == "") return "None";
            else 
            return rewards;
        }
    }
}
