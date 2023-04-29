using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using System;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        // This class handles the coordination of different player functionalities 

        private int layerUI;
        private bool isDraggingUI = false;

        private Mover mover;
        private Fighter fighter;
        private Health health;

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public float offsetDivisionValue;
            //public Vector2 defaultHotspot;

            public Vector2 GetHotspot()
            {                
                return new Vector2(texture.width / offsetDivisionValue, texture.height / offsetDivisionValue);   
            }
        }

        [Header("NavMesh paths parameters")]
        [SerializeField] float maxNavMeshProjectionDistance = 1.0f;
        [Header("Cursor settings")]
        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float raycastRadius = 0.5f;

        void Awake()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();

            layerUI = LayerMask.NameToLayer("UI");
        }

        void Update()
        {
            // By order of priority
            if (InteractWithUI()) return;
            if (health.IsDead())
            {
                SetCursor(CursorType.Default);
                return;
            }

            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;

            SetCursor(CursorType.Default);
        }
        private bool InteractWithUI()
        {
            if (Input.GetMouseButtonUp(0))
            {
                isDraggingUI = false;
            }
            
            if (EventSystem.current.IsPointerOverGameObject()) // This function only checks UI
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isDraggingUI = true;
                }
                SetCursor(CursorType.UI);
                return true;
            }

            if (isDraggingUI)
            {
                return true;
            }
            return false;
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();

            foreach (RaycastHit hit in hits)
            {
                IRaycastable [] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType()); 
                        return true;
                    }
                }
            }
            return false;
        }

        private RaycastHit[] RaycastAllSorted()
        {
            // Get all hits
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);

            // Build the array of distances
            float[] distances = new float[hits.Length];
            // Populate array
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            // Sort hits by distance
            Array.Sort(distances, hits);
            // Return sorted array
            return hits;
        }

        private bool InteractWithMovement()
        {
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);

            if (hasHit)
            {
                if (!mover.CanMoveTo(target)) return false;
                
                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(target);
                }

                // if successful, will change the cursor to movement and return true
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            // Set default value for target
            target = new Vector3();
            
            // Raycast to terrain
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            // If it fails, return early
            if (!hasHit) return false;

            // Find neareast navmesh point
            NavMeshHit navMeshHit;
            bool hasHitNavmesh = NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasHitNavmesh) return false;

            target = navMeshHit.position;

            return true;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.GetHotspot(), CursorMode.Auto); ;
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }

        private static Ray GetMouseRay()
        {
            // returns mouse position, in world 
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

    }
}

