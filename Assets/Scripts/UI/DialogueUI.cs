using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Dialogue;
using TMPro;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField] TextMeshProUGUI AIText;
        [SerializeField] Button nextButton;

        void Start()
        {
            // Get reference to playerConversant
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            // Setting up event for button click
            nextButton.onClick.AddListener(Next);

            UpdateUI();
        }

        void Next()
        {
            playerConversant.Next();
            UpdateUI();
        }

        private void UpdateUI()
        {
            AIText.text = playerConversant.GetText();
            // Only show Next button if there are children nodes left after current one
            nextButton.gameObject.SetActive(playerConversant.HasNext());
        }
    }
}