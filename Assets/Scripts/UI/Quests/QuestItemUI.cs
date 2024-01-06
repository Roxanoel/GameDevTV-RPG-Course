using System.Collections;
using System.Collections.Generic;
using TMPro;
using RPG.Quests;
using UnityEngine;

public class QuestItemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI progress;
    
    public void Setup(Quest quest)
    {
        title.text = quest.name;
        progress.text = $"{quest.GetObjectivesNumber()}/{quest.GetObjectivesNumber()}"; // FOR NOW
    }
}
