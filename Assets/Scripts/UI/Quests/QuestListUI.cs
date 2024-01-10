using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using UnityEngine;

public class QuestListUI : MonoBehaviour
{
    [SerializeField] QuestItemUI questPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        // Get a hold of the player
        QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
        // Ensure we clear the list before populating it
        transform.DetachChildren();
        foreach(QuestStatus status in questList.GetStatuses())
        {
            QuestItemUI questItemUI = Instantiate(questPrefab, this.transform);
            questItemUI.Setup(status);
        }
    }

}
