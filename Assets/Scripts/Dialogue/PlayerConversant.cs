using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialogue testDialogue;
        Dialogue currentDialogue; // In the future this will be changed/set programmatically
        DialogueNode currentNode = null;
        AIConversant currentConversant = null;
        bool showingReplies = false;

        public event Action onConversationUpdated;

        public void StartDialogue(AIConversant newConversant, Dialogue newDialogue)
        {
            currentConversant = newConversant;
            currentDialogue = newDialogue;
            // Initialise currentNode as root node of current dialogue
            currentNode = currentDialogue.GetRootNode();
            // Trigger events
            TriggerEnterAction();
            onConversationUpdated();
        }

        public bool IsActive()
        {
            return currentDialogue != null;
        }

        public void Quit()
        {
            TriggerExitAction();

            currentConversant = null;
            currentDialogue = null;
            currentNode = null;
            showingReplies = false;
            onConversationUpdated();
        }

        public bool ShowingReplies()
        {
            return FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode)).Count() > 0;  
        }

        public string GetText()
        {
            // safeguard 
            if (currentNode == null)
            {
                return "";
            }
            return currentNode.GetText(); 
        }

        // Called when we want to progress the dialogue 
        public void Next()
        {
            if (!HasNext())
            {
                Debug.Log("reached end of dialogue branch");
                onConversationUpdated(); // For good measure 
                return;
            }
            DialogueNode[] children = FilterOnCondition(currentDialogue.GetAIChildren(currentNode)).ToArray();
            // Trigger exist action before changing the node
            TriggerExitAction();
            // For now, returning a random child node 
            currentNode = children[UnityEngine.Random.Range(0, children.Length)];
            // trigger event + enter action
            TriggerEnterAction();
            onConversationUpdated();
        }

        // Tells you if you have reached a leaf node of the dialogue tree
        public bool HasNext()
        {
            return FilterOnCondition(currentDialogue.GetAIChildren(currentNode)).Count() > 0;
        }

        private IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNodes)
        {
            return inputNodes; //for now
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            return FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode));
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            // Update currentNode
            currentNode = chosenNode;
            // Trigger enter action
            TriggerEnterAction();
            // Don't display the text of the selected node again
            Next();
        }

        private void TriggerEnterAction()
        {
            if (currentNode != null)
            {
                TriggerAction(currentNode.GetOnEnterAction());
            }
        }
        private void TriggerExitAction()
        {
            if (currentNode != null)
            {
                TriggerAction(currentNode.GetOnExitAction());
            }
        }

        private void TriggerAction(DialogueTriggerType action)
        {
            if (action == DialogueTriggerType.None || currentConversant == null) return;

            foreach (DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger>())
            {
                trigger.Trigger(action);
            }
        }

        public string GetCurrentConversantName()
        {
            return currentConversant.GetConversantName();
        }
    }
}

