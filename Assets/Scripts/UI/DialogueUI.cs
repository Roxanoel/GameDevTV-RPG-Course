using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue;
using TMPro;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField] TextMeshProUGUI AIText;

        void Start()
        {
            // Get reference to playerConversant
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            // Set the text in the UI based on dialogue nodes
            AIText.text = playerConversant.GetText();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}