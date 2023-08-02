using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialogue currentDialogue; // In the future this will be changed/set programmatically

        public string GetText()
        {
            // safeguard 
            if (currentDialogue == null)
            {
                return "";
            }
            return currentDialogue.GetRootNode().GetText(); // Currently hardcoded but eventually could make the function more flexible to return text of desired node
        }

        // Called when we want to progress the dialogue 
        public void Next()
        {

        }

        // Tells you if you have reached a leaf node of the dialogue tree
        public bool HasNext()
        {
            return true; // temp so that it compiles
        }
    }
}

