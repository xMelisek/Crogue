using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAI : BaseAI
{
    public float health = 10f;
    public float damage = 15f;
    public float moveSpeed = 0.5f;
    Transform target;

    private void FixedUpdate()
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;
        else
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var obj = collision.gameObject;
        if (obj.CompareTag("Player") && !obj.GetComponent<PlayerBehaviour>().invincible)
            obj.GetComponent<PlayerBehaviour>().TakeDamage(damage);
    }

    public override void TakeDamage(float dmgVal)
    {
        health -= dmgVal;
        if (health <= 0) Die();
    }

    public override void Die()
    {
        Destroy(gameObject);
    }
}
