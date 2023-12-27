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
        [SerializeField] Transform repliesRoot;
        [SerializeField] GameObject replyPrefab;

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
                    UpdateUI(); // Won't that cause a loop? If not, how so?
                });
            }
        }

        private void UpdateUI()
        {
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