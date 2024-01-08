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
        
        public void Setup(Quest quest)
        {
            Debug.Log(quest);
        }
    }
}
