using System.Collections;
using System.Collections.Generic;
using TMPro;
using RPG.Quests;
using UnityEngine;

public class QuestItemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI progress;

    Quest currentQuest;
    
    public void Setup(Quest quest)
    {
        currentQuest = quest;
        title.text = quest.GetTitle();
        progress.text = $"{quest.GetObjectivesCount()}/{quest.GetObjectivesCount()}"; // FOR NOW
    }

    public Quest GetQuest()
    {
        return currentQuest;
    }
}
