using System.Collections;
using System.Collections.Generic;
using TMPro;
using RPG.Quests;
using UnityEngine;

public class QuestItemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI progress;

    QuestStatus currentQuestStatus;
    Quest currentQuest;
    
    public void Setup(QuestStatus status)
    {
        currentQuestStatus = status;
        currentQuest = status.GetQuest();
        title.text = currentQuest.GetTitle();
        progress.text = $"{status.GetCompletedObjectivesCount()}/{currentQuest.GetObjectivesCount()}"; // FOR NOW
    }

    public Quest GetQuest()
    {
        return currentQuest;
    }

    public QuestStatus GetQuestStatus()
    {
        return currentQuestStatus;
    }
}
