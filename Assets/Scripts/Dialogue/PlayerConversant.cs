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
        bool showingReplies = false;

        public event Action onConversationUpdated;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(2);
            StartDialogue(testDialogue);
        }

        public void StartDialogue(Dialogue newDialogue)
        {
            currentDialogue = newDialogue;
            // Initialise currentNode as root node of current dialogue
            currentNode = currentDialogue.GetRootNode();
            // Trigger event
            onConversationUpdated();
        }

        public bool IsActive()
        {
            return currentDialogue != null;
        }

        public void Quit()
        {
            currentDialogue = null;
            currentNode = null;
            showingReplies = false;
            onConversationUpdated();
        }

        public bool ShowingReplies()
        {
            return currentDialogue.GetPlayerChildren(currentNode).Count() > 0;  
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
            DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
            // For now, returning a random child node 
            currentNode = children[UnityEngine.Random.Range(0, children.Length)];
            // trigger event
            onConversationUpdated();
        }

        // Tells you if you have reached a leaf node of the dialogue tree
        public bool HasNext()
        {
            return currentDialogue.GetAIChildren(currentNode).Count() > 0;
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            return currentDialogue.GetPlayerChildren(currentNode);
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            // Update currentNode
            currentNode = chosenNode;
            // Don't display the text of the selected node again
            Next();
        }
    }
}

