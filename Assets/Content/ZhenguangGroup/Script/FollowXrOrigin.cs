using UnityEngine;

public class FollowXrOrigin : MonoBehaviour
{
    private Transform xrOriginTransform;

    void Start()
    {
        GameObject xrOrigin = GameObject.FindGameObjectWithTag("MainCamera");

        if (xrOrigin != null)

        {
            xrOriginTransform = xrOrigin.transform;
            UnityEngine.Debug.Log("xrOrigin found at position: " + xrOriginTransform.position);
        }
        else
        {
            UnityEngine.Debug.LogError("GameObject named 'xrOrigin' not found in the scene!");
        }
    }

    void Update()
    {
        if (xrOriginTransform != null)
        {
            // Set x and z to follow xrOrigin, and keep y fixed at 0.1
            transform.position = new Vector3(xrOriginTransform.position.x, -1.4f, xrOriginTransform.position.z);
            UnityEngine.Debug.Log("Position updated to: " + transform.position);
        }
    }
}
