using UnityEngine;
using RPG.Attributes;
using RPG.Stats;
using GameDevTV.Inventories;
using System.Collections.Generic;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/ Make new Weapon", order = 0)]
    public class WeaponConfig : EquipableItem, IModifierProvider
    {
        // Scriptable object storing weapon config and methods
        // - Spawning weapons in the appropriate hand
        // - Switching weapons
        // - Launching projectiles
        // - Getters for config parameters
        
        [SerializeField] Weapon equippedPrefab = null;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] float weaponRange = 2.0f;
        [SerializeField] float weaponDamage = 1.0f;
        [SerializeField] float percentageBonus = 0.0f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";

        public Weapon Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
        {
            DestroyOldWeapon(rightHandTransform, leftHandTransform);

            Weapon weapon = null;

            // If a prefab is given, spawn that prefab in the correct hand
            if (equippedPrefab != null)
            {
                Transform handTransform = GetHandTransform(rightHandTransform, leftHandTransform);
                weapon = Instantiate(equippedPrefab, handTransform);
                weapon.name = weaponName;
            }

            // Stores "default" animator controller
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            // Set animator override controller to the animation matching the weapon
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            // If there is no override, set animation controller to default
            else if (overrideController != null)
            {    
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
            // Return the weapon prefab that was spawned
            return weapon;

        }

        private void DestroyOldWeapon(Transform rightHandTransform, Transform leftHandTransform)
        {
            // Search for weapon in both hands
            Transform oldWeapon = rightHandTransform.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHandTransform.Find(weaponName);
            }
            if (oldWeapon == null) return;

            // If a weapon is found, destroy it
            oldWeapon.name = "TO BE DESTROYED";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetHandTransform(Transform rightHandTransform, Transform leftHandTransform)
        {
            // Checks if the weapon is right handed or not, and return the corresponding hand transform
            return isRightHanded ? rightHandTransform : leftHandTransform;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, GameObject instigator, Health target, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }

        public float GetRange()
        {
            return weaponRange;
        }

        public float GetPercentageBonus()
        {
            return percentageBonus;
        }

        public float GetDamage()
        {
            return weaponDamage;
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return weaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return percentageBonus;
            }
        }
    }
}
