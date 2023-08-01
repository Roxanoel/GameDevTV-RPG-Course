using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialogue currentDialogue; // In the future this will be changed/set programmatically

        void Start()
        {
        
        }

        void Update()
        {
        
        }

        public string GetText()
        {
            // safeguard 
            if (currentDialogue == null)
            {
                return "";
            }
            return currentDialogue.GetRootNode().GetText(); // Currently hardcoded but eventually make the function more flexible to return text of desired node
        }
    }
}

