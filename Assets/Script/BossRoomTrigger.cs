using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    private BossController _boss;

    public void Init(BossController boss, BoundsInt room)
    {
        _boss = boss;

        // Créer un collider trigger à la taille exacte de la room
        var col = gameObject.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        col.size = new Vector2(room.size.x, room.size.y);
        col.offset = Vector2.zero;

        transform.position = room.center;
        transform.position = new Vector3(room.center.x, room.center.y, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _boss.OnPlayerEnterRoom();
            Debug.Log("[BossRoom] Player entré !");
        }
    }
}