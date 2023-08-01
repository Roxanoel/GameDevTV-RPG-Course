using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;

        void Start()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}