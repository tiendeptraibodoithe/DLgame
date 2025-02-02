using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    PlayerHealth target;
    [SerializeField] float damage = 40f;

    void Start()
    {
        target = FindObjectOfType<PlayerHealth>();
    }

    public void OnDamageTaken()
    {
        Debug.Log("cc");
    }
    public void AttackHitEvent()
    {
        if(target == null) return;
        target.PlayerTakeDamge(damage);
        Debug.Log("Bang Bang");
    }
}
