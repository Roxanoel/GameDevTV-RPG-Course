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
        [SerializeField] Button quitButton;
        [SerializeField] Transform repliesRoot;
        [SerializeField] GameObject replyPrefab;
        [SerializeField] TextMeshProUGUI conversantName;

        void Start()
        {
            // Get reference to playerConversant
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            // Setting up event listener for conversation changes
            playerConversant.onConversationUpdated += UpdateUI;
            // Setting up event for button click
            nextButton.onClick.AddListener(Next);
            // Setting up quit button
            quitButton.onClick.AddListener(playerConversant.Quit);

            UpdateUI();
        }

        void Next()
        {
            playerConversant.Next();
        }

        private void BuildChoiceList()    // In the course, reply = choice, and ShowingReplies = IsChoosing
        {
            // Only show replies if ShowingReplies is true
            repliesRoot.gameObject.SetActive(playerConversant.ShowingReplies());

            // Update replies based on PlayerConversant
            foreach (DialogueNode reply in playerConversant.GetChoices())
            {
                GameObject replyInstance = Instantiate(replyPrefab, repliesRoot);
                replyInstance.GetComponentInChildren<TextMeshProUGUI>().text = reply.GetText();
                // Get a hold of the button component 
                Button button = replyInstance.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => {
                    playerConversant.SelectChoice(reply);
                });
            }
        }

        private void UpdateUI()
        {
            gameObject.SetActive(playerConversant.IsActive());
            
            // If there is no dialogue selected, do nothing
            if (!playerConversant.IsActive())
            {
                return;
            }

            conversantName.text = playerConversant.GetCurrentConversantName();
            AIText.text = playerConversant.GetText();

            // Clear replies
            foreach (Transform item in repliesRoot)
            {
                Destroy(item.gameObject);
            }

            if (playerConversant.ShowingReplies())
            {
                BuildChoiceList();
            }

            // Only show Next button if there are children nodes left after current one, and there are no replies
            nextButton.gameObject.SetActive(playerConversant.HasNext() && !playerConversant.ShowingReplies());
           
        }
    }
}