using System.Collections;
using System.Collections.Generic;
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
        
        public void Setup(QuestStatus status)
        {
            Quest quest = status.GetQuest();
            title.text = quest.GetTitle();
            // Before populating the list of objectives make sure it's empty
            foreach (Transform item in objectivesContainer)
            {
                Destroy(item.gameObject);
            }

            foreach (string objective in quest.GetObjectivesList())
            {
                Debug.Log(objective);
                GameObject objectiveInstance = Instantiate(objectivePrefab, objectivesContainer) as GameObject;
                // Setup the objective
                QuestObjectiveUI objectiveUI = objectiveInstance.GetComponent<QuestObjectiveUI>();
                objectiveUI.SetTitleText(objective);
            }
        }
    }
}
