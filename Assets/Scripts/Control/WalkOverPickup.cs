using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Inventories;
using RPG.Movement;

namespace RPG.Control
{
    public class WalkOverPickup : Pickup, IRaycastable
    {
        private bool isInRange = false;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isInRange = true;
                HandleRaycast(other.GetComponent<PlayerController>());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isInRange = false;
                HandleRaycast(other.GetComponent<PlayerController>());
            }
        }
        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(isInRange)
                {
                    PickupItem();
                }
                else
                {
                    Mover mover = callingController.GetComponent<Mover>();
                    mover.StartMoveAction(transform.position);
                }
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            // different cursor type to reflect whether the item is in range to be picked up or not
            if (isInRange == false)
            {
                return CursorType.Movement;
            }
            return CursorType.Item;
        }

    }
}
