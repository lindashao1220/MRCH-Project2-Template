using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PathCreator : MonoBehaviour
{
    public Transform startObject;   // The starting point of the path
    public Transform endObject;     // The ending point of the path
    public int points = 20;         // Number of points along the path for smoothness

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (startObject == null || endObject == null)
        {
            Debug.LogError("Please assign start and end objects.");
            return;
        }

        CreatePath();
    }

    void CreatePath()
    {
        // Set the number of points in the LineRenderer
        lineRenderer.positionCount = points;

        // Loop through each point and calculate its position
        for (int i = 0; i < points; i++)
        {
            float t = i / (float)(points - 1);
            Vector3 pointPosition = Vector3.Lerp(startObject.position, endObject.position, t);
            lineRenderer.SetPosition(i, pointPosition);
        }
    }

    void Update()
    {
        // Update the path if either object moves
        if (startObject.hasChanged || endObject.hasChanged)
        {
            CreatePath();
        }
    }
}