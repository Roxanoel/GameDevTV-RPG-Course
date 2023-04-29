using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Attributes;
using RPG.Movement;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        // This class contains the functionality for weapon pickups 
        // - Trigger collider & calling methods to equip weapon on player
        // - Disappearing after picking up
        // - Respawning after configurable delay
        
        [SerializeField] WeaponConfig weapon;
        [SerializeField] float healthToRestore = 0.0f;
        [SerializeField] float respawnDelay = 5.0f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Pickup(other.gameObject);
            }
        }

        private void Pickup(GameObject subject)
        {
            if (weapon != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(weapon);
            }
            if (healthToRestore > 0)
            {
                subject.GetComponent<Health>().Heal(healthToRestore);
            }
            StartCoroutine(HideForSeconds(respawnDelay));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {               
            if (Input.GetMouseButtonDown(0))
            {
                Mover mover = callingController.GetComponent<Mover>();
                mover.StartMoveAction(transform.position);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Item;
        }
    }
}

