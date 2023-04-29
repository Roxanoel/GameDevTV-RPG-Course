using RPG.Attributes;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        // This class manages projectile behaviours.
        // - Setting aim for the projectile
        // - homing or not
        // - Speed
        // - vfx
        
        [SerializeField] float speed = 1;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject hitFX = null;
        [SerializeField] float maxLifeTime = 10.0f;
        [SerializeField] float lifetimeAfterImpact = 2.0f;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] UnityEvent onHit;

        Health target = null;
        GameObject instigator = null;
        float damage = 0;

        private void Start()
        {
            SetAimDirection();
        }

        private void Update()
        {
            if (target == null) return;

            // If homing, the projectile's aim will be adjusted to the target every frame
            if (isHoming && !target.IsDead()) 
            { 
                SetAimDirection(); 
            }

            // Move arrow towards direction of aim
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private void SetAimDirection()
        {
            transform.LookAt(GetAimLocation());
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();

            // If the target has no capsule collider, aim for its transform.position
            if (targetCapsule == null) { return target.transform.position; }

            // If the target has a capsule collider, aim for the middle of the capsule's height
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            // If the collider that is hit is not the target, do nothing
            if (other.GetComponent<Health>() != target) return;
            // If the target was already dead, do nothing
            if (target.IsDead()) return;

            // Stop arrow movement
            speed = 0;

            // Play hit sound
            onHit.Invoke();

            // Instantiate VFX and destroy it once it has played
            if (hitFX != null)
            {
                SpawnAndDestroyHitEffect();
            }

            target.TakeDamage(instigator, damage);

            // Destroys any elements of the projectile identified as needing to be destroyed on hit
            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            // Destroys the projectile itself after a configurable delay
            Destroy(gameObject, lifetimeAfterImpact);
        }

        private void SpawnAndDestroyHitEffect()
        {
            // Instantiate the VFX at the location which was aimed for/hit
            Instantiate(hitFX, GetAimLocation(), Quaternion.identity);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;

            // Ensures that all projectiles get destroyed after a configurable maximum duration (garbage collection) 
            Destroy(gameObject, maxLifeTime);
        }
    }
}
