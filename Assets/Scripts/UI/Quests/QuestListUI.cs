using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using UnityEngine;

public class QuestListUI : MonoBehaviour
{
    [SerializeField] Quest[] tempQuests;
    [SerializeField] QuestItemUI questPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        // Ensure we clear the list before populating it
        transform.DetachChildren();
        foreach(Quest quest in tempQuests)
        {
            QuestItemUI questItemUI = Instantiate(questPrefab, this.transform);
            questItemUI.Setup(quest);
        }
    }

}
