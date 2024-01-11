using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Quests;
using UnityEngine;

public class QuestListUI : MonoBehaviour
{
    [SerializeField] QuestItemUI questPrefab;
    QuestList questList;
    
    // Start is called before the first frame update
    void Start()
    {
        // Get a hold of the player
        questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
        questList.onListUpdated += Redraw;
        Redraw();
    }

    private void Redraw()
    {
        // Ensure we clear the list before populating it
        transform.DetachChildren();
        // Populate the list
        foreach (QuestStatus status in questList.GetStatuses())
        {
            QuestItemUI questItemUI = Instantiate(questPrefab, this.transform);
            questItemUI.Setup(status);
        }
    }
}
