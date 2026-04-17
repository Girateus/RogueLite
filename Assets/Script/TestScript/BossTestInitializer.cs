using UnityEngine;

public class BossTestInitializer : MonoBehaviour
{
    [SerializeField] private BossController _boss;

    void Awake()
    {
        if (_boss == null) _boss = GetComponent<BossController>();
        
        // We create a fake BoundsInt so the boss doesn't throw NullRef
        // if its logic depends on _ctx.Room
        BoundsInt fakeRoom = new BoundsInt(
            Vector3Int.FloorToInt(_boss.transform.position) - new Vector3Int(10, 10, 0),
            new Vector3Int(20, 20, 1)
        );

        // We use the existing SetRoom from your original script
        _boss.SetRoom(fakeRoom);
        
        Debug.Log($"<color=green>[TestMode]</color> {_boss.name} initialized with dummy room data.");
    }
}