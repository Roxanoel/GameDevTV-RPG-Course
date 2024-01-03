using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat
{
    // Marks a GameObject as a possible target for combat
    // Check if attack is possible and initiate it

    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {

        public bool HandleRaycast(PlayerController callingController)
        {
            if (!enabled) return false;

            Fighter fighter = callingController.GetComponent<Fighter>();

            // Return false if target cannot be attacked
            if (!fighter.CanAttack(this.gameObject)) return false;

            // If these checks are passed and left click, attack target
            if (Input.GetMouseButton(0))
            {
                fighter.Attack(this.gameObject);
            }
            return true;
        }
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }
    }

}
