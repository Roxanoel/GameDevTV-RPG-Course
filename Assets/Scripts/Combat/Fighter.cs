
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;
using GameDevTV.Saving;
using GameDevTV.Inventories;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        // This class manages all fighting behaviour (enemies and player alike)
        // - Equipping/unequipping weapons
        // - Attack behaviour (with configurable time between attacks)
        // - Stores and updates target of attack
        
        // Config
        [SerializeField] float timeBetweenAttacks = 1.0f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeaponConfig = null;

        // Cache
        Animator animator;
        Health target;
        Equipment equipment;
        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;
        float timeSinceLastAttack = Mathf.Infinity;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            currentWeaponConfig = defaultWeaponConfig;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);

            equipment = GetComponent<Equipment>();
            if (equipment)
            {
                equipment.equipmentUpdated += UpdateWeapon;
            }
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeaponConfig);
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            bool isInAttack = animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");

            if (target == null) return;

            if (target.IsDead()) return;

            // If target is not in range, moves to it. If in range, attack it.
            if (!IsInRange(target.transform) && !isInAttack)
            {
                GetComponent<Mover>().MoveTo(target.transform.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }

        }

        private void AttackBehaviour()
        {
            // Makes Fighter face its target
            transform.LookAt(target.transform);

            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // This will trigger the Hit() event.
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            animator.ResetTrigger("cancelAttack");
            animator.SetTrigger("attack");
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;

            if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position)
                && IsInRange(combatTarget.transform)) 
                return false;

            Health targetToTest = combatTarget.GetComponent<Health>();

            // Will return "true" iff there is a target (with a health component) which is not already dead
            return targetToTest != null && !targetToTest.IsDead();
        }
        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            // Resets target
            target = null; 

            StopAttack();
            GetComponent<Mover>().Cancel();
        }

        // This stops the attack animation, which also stops the anim even Hit()
        private void StopAttack()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("cancelAttack");
        }

        // Animation event
        void Hit()
        {
            if (target == null) return;

            // This calculates damage including any modifiers, in BaseStats class
            float calculatedDamage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if (currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }
            else
            {
                Debug.LogWarning($"{name} does not have a physical weapon attached");
            }
            
            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, gameObject, target, calculatedDamage);
            }
            else
            {                
                target.TakeDamage(gameObject, calculatedDamage);
            }
        }

        void Shoot()
        {
            Hit();
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            // Matches the config to the weapon to equip
            currentWeaponConfig = weapon;
            // Equips the weapon prefab
            currentWeapon.value = AttachWeapon(weapon);
        }

        private void UpdateWeapon()
        {
            // Get weapon from equipment & cast it as WeaponConfig
            var equipedWeapon = (WeaponConfig)equipment.GetItemInSlot(EquipLocation.Weapon);
            // If there is nothing in the weapon slot, equiped default weapon
            if (equipedWeapon == null)
            {
                EquipWeapon(defaultWeaponConfig);
            }
            // Else (if a weapon is in the slot) equip weapon
            else
            {
                EquipWeapon(equipedWeapon);
            }
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }
        public Health GetTarget()
        {
            return target;
        }

        private bool IsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) <= currentWeaponConfig.GetRange();
        }

        // ISaveable methods
        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }

    }
}