using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        // This class is responsible for updating the health display.
        // It does so through events rather than update.
        
        Health health;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void OnEnable()
        {
            health.onHealthChanged += UpdateHealthDisplay;
        }

        private void OnDisable()
        {
            health.onHealthChanged -= UpdateHealthDisplay;
        }

        private void UpdateHealthDisplay()
        {
            GetComponent<Text>().text = $"{health.GetHealthPoints():0}/{health.GetMaxHealthPoints():0}";
        }
    }
}
