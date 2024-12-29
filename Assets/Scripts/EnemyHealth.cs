using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float hitPoints = 100f;

    // Thêm reference đến VaccineCollectionSystem
    private VaccineCollectionSystem missionSystem;

    private void Start()
    {
        // Tìm mission system trong scene
        missionSystem = FindObjectOfType<VaccineCollectionSystem>();
    }

    public void TakeDamage(float damage)
    {
        BroadcastMessage("OnDamageTaken", damage);
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            // Thông báo cho mission system khi zombie chết
            if (missionSystem != null)
            {
                missionSystem.OnZombieKilled();
            }
            Destroy(gameObject);
        }
    }
}