using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialogue currentDialogue; // In the future this will be changed/set programmatically
        DialogueNode currentNode = null;
        bool showingReplies = false;

        private void Awake()
        {
            // Initialise currentNode as root node of current dialogue
            currentNode = currentDialogue.GetRootNode();
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
            DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
            // For now, returning a random child node 
            currentNode = children[Random.Range(0, children.Length)];
        }

        // Tells you if you have reached a leaf node of the dialogue tree
        public bool HasNext()
        {
            return currentDialogue.GetAIChildren(currentNode).Count() > 0;
        }

        public IEnumerable<string> GetChoices()
        {
            yield return "mock reply 1";
            yield return "mock reply 2!!";
            yield return "last mock reply, woof";
        }
    }
}

