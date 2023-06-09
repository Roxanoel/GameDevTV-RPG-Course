﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay: MonoBehaviour
    {
        // Updates experience display through events

        Experience experience;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            GetComponent<Text>().text = $"Experience: {experience.GetExperiencePoints():0}";
        }
    }
}