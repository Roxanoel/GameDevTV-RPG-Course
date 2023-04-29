using UnityEngine;
using UnityEngine.UI;
using RPG.Attributes;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        // Responsible for updating the enemy health display.
        // Uses update; could be made more performant if I find a way to use events instead, but this will require looking into target acquisition
        
        Health health;
        Fighter fighter;
        
        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            health = fighter.GetTarget();
            if (health == null) 
            {
                GetComponent<Text>().text = "";
            }
            else
            {
                GetComponent<Text>().text = $"{health.GetHealthPoints():0}/{health.GetMaxHealthPoints():0}";
            }             
        }

    }
}
