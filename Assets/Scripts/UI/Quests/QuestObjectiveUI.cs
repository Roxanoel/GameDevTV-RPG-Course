using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestObjectiveUI : MonoBehaviour
    {
        [SerializeField] GameObject completedIcon;
        [SerializeField] GameObject incompleteIcon;
        [SerializeField] TextMeshProUGUI title;

        public void SetCompletionStatus(bool isComplete)
        {
            completedIcon.SetActive(isComplete);
            incompleteIcon.SetActive(!isComplete);
        }

        public void SetTitleText(string text)
        {
            title.text = text;
        }
    }
}
