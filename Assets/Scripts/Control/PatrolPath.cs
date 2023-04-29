using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        // Guard paths gizmos, getter for waypoint positions, method for cycling between waypoints

        const float waypointGizmoSize = 0.25f;

        public Vector3 GetWaypointPosition(int i)
        {
            return transform.GetChild(i).position;
        }

        public int GetNextIndex(int i)
        {
            if (i < transform.childCount - 1) // -1 accounts for the fact that i is an index
            {
                return i + 1;
            }
            else
            {
                return 0;
            }
        }

        //Called by Unity
        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(GetWaypointPosition(i), waypointGizmoSize);
                Gizmos.DrawLine(GetWaypointPosition(i), GetWaypointPosition(GetNextIndex(i)));
            }
        }
    }
}
