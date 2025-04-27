using UnityEngine;

// Attach this to each SpawnPoint object to show a circle and label in the Scene view.
public class SpawnPointGizmo : MonoBehaviour
{
    public float radius = 0.5f;                     // Radius of the wireframe circle to be drawn around the spawn point.
    public Color gizmoColor = Color.red;            // Colour used to draw the gizmo in the Scene view.

    // Called by Unity Editor to draw Gizmos in the Scene view.
    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;                          // Set the Gizmos colour to the chosen colour.
        Gizmos.DrawWireSphere(transform.position, radius);  // Draw a wireframe sphere at the object's position.

    // Draw a label above the spawn point, but only inside the Unity Editor.
#if UNITY_EDITOR
        UnityEditor.Handles.color = gizmoColor;     // Set the label colour to match the gizmo.
        UnityEditor.Handles.Label(
            transform.position + Vector3.up * 1f,   // Position the label slightly above the spawn point.
            gameObject.name                         // Use the GameObject's name as the label text.
        );
#endif
    }
}
