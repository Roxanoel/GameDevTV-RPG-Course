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

        private void Awake()
        {
            // Initialise currentNode as root node of current dialogue
            currentNode = currentDialogue.GetRootNode();
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
            DialogueNode[] children = currentDialogue.GetAllChildren(currentNode).ToArray();
            // For now, always using the first child
            currentNode = children[0];
        }

        // Tells you if you have reached a leaf node of the dialogue tree
        public bool HasNext()
        {
            return true; // temp so that it compiles
        }
    }
}

