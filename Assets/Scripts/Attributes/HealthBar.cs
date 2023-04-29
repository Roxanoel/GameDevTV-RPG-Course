using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Health healthComponent = null;
        [SerializeField] GameObject healthBarCanvas = null;

        private void OnEnable()
        {
            if (healthComponent != null)
            {
                healthComponent.onHealthChanged += UpdateHealthBar;
            }
        }
        private void OnDisable()
        {
            if (healthComponent != null)
            {
                healthComponent.onHealthChanged -= UpdateHealthBar;
            }
        }

        private void UpdateHealthBar()
        {
            float healthFraction = healthComponent.GetFraction();

            // Only display the health bar is life is not full, nor empty
            if (Mathf.Approximately(healthFraction, 0) || Mathf.Approximately(healthFraction, 1))
            {
                healthBarCanvas.SetActive(false);
            }
            else
            {
                healthBarCanvas.SetActive(true);
                foreground.localScale = new Vector3(healthFraction, 1, 1);
            }

        }
    }
}
