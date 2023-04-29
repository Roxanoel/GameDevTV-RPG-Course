using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using GameDevTV.Utils;

namespace RPG.Control
{
    /// <summary>
    ///Controls behaviours of NPCs, currently only enemies
    /// - Logic for switching between behaviours: Guarding, Patrolling, Suspicion
    /// - Timers for behaviours
    /// - Gizmos for chase radius
    /// </summary>
    public class AIController : MonoBehaviour
    {


        [Header("Chase parameters")]
        [SerializeField] float chaseDistance = 5.0f;
        [SerializeField] float chaseSpeed = 4.5f;
        [SerializeField] float suspicionStateDuration = 3.0f;
        [Header("Patrolling parameters")]
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float patrolSpeed = 3.0f;
        [SerializeField] float waypointTolerance = 2.6f;
        [SerializeField] float waypointDwellTime = 3.0f;
        [Header("Aggravation")]
        [SerializeField] float aggravationDuration = 3.0f;
        [SerializeField] float shoutDistance = 5.0f;

        private GameObject player;
        private Fighter fighter;
        private Health health;
        private Mover mover;

        LazyValue<Vector3> guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceAggravated = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();

            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void Start()
        {
            guardPosition.ForceInit();
        }
        private void Update()
        {
            // Logic for determining which behaviour to adopt 

            if (health.IsDead()) return;
            if (IsAggravated() && fighter.CanAttack(player.gameObject))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionStateDuration)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        public void Aggravate()
        {
            // Sets timer
            timeSinceAggravated = 0;
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceAggravated += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            mover.SetSpeed(patrolSpeed);
            Vector3 nextPosition = guardPosition.value;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoints();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition);
            }
        }
        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }
        private void CycleWaypoints()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }
        
        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypointPosition(currentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            // Suspicion state = cancelling actions (no moving or chasing; idle)
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            // Chases, then attacks when in weapon range
            mover.SetSpeed(chaseSpeed);
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player.gameObject);

            AggravateNearbyEnemies();
        }

        private void AggravateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);

            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.transform.GetComponent<AIController>();
                if ( ai != null)
                {
                    ai.Aggravate();
                }
            }
        }

        private bool IsAggravated()
        {
            float distanceToPlayer = Vector3.Distance(gameObject.transform.position, player.transform.position);
            // Check if player is closer than chase distance OR if aggravated
            return distanceToPlayer < chaseDistance || timeSinceAggravated < aggravationDuration; 
        }


        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(gameObject.transform.position, chaseDistance);
        }

    }
}

