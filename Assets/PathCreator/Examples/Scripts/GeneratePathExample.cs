using UnityEngine;
using PathCreation;

namespace PathCreation.Examples
{
    // Example of creating a path at runtime from a set of points and continuously updating it.
    [RequireComponent(typeof(PathCreator))]
    public class GeneratePathExample : MonoBehaviour
    {
        public bool closedLoop = true;
        public Transform[] waypoints;

        private PathCreator pathCreator;
        private BezierPath bezierPath;

        void Start()
        {
            pathCreator = GetComponent<PathCreator>();
            UpdatePath();
        }

        void Update()
        {
            // Check if any waypoint positions have changed, then update the path
            UpdatePath();
        }

        void UpdatePath()
        {
            if (waypoints.Length > 0)
            {
                // Create a new bezier path from the current waypoint positions
                Vector3[] points = new Vector3[waypoints.Length];
                for (int i = 0; i < waypoints.Length; i++)
                {
                    points[i] = waypoints[i].position;
                }

                bezierPath = new BezierPath(points, closedLoop, PathSpace.xyz);
                pathCreator.bezierPath = bezierPath;
            }
        }
    }
}
