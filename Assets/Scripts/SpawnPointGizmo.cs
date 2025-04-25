using UnityEngine;

// Attach this to each SpawnPoint object to show a circle and label in the Scene view.
public class SpawnPointGizmo : MonoBehaviour
{
    public float radius = 0.5f;                     // Radius of the wireframe circle
    public Color gizmoColor = Color.red;            // Colour of the gizmo

    void OnDrawGizmos()
    {
        // Draw the wireframe circle
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, radius);

        // Draw a label above the spawn point
#if UNITY_EDITOR
        UnityEditor.Handles.color = gizmoColor;
        UnityEditor.Handles.Label(
            transform.position + Vector3.up * 1f,  // Offset the label slightly above
            gameObject.name                        // Use the GameObject's name as label
        );
#endif
    }
}
