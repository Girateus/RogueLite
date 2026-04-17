using UnityEngine;

public class ManualRoomActivator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private BossController _targetBoss;
    [SerializeField] private bool _deactivateOnExit = true;

    // This script assumes the GameObject has a Trigger Collider attached
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _targetBoss != null)
        {
            Debug.Log($"<color=cyan>[TestMode]</color> Player entered zone. Activating {_targetBoss.name}");
            _targetBoss.OnPlayerEnterRoom();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _targetBoss != null && _deactivateOnExit)
        {
            Debug.Log($"<color=orange>[TestMode]</color> Player left zone. Deactivating {_targetBoss.name}");
            
            // Accessing the private _machine field via a 'trick' since we don't want to modify BossController
            // For a test script, we can use a forced reset or simply let the boss idle.
            // If you can't access private members, a simple way is to just stop the logic:
            _targetBoss.enabled = false; 
            // Note: In a real scenario, adding a Public Deactivate() to BossController would be cleaner.
        }
    }

    // Helper to see the zone in the Scene View
    private void OnDrawGizmos()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box == null) return;

        Gizmos.color = new Color(0, 1, 1, 0.15f);
        Gizmos.DrawCube(transform.TransformPoint(box.offset), box.size);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.TransformPoint(box.offset), box.size);
    }
}