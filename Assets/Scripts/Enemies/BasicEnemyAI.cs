using UnityEngine;

public class BasicEnemyAI : BaseAI
{
    public float health = 10f;
    public float damage = 15f;
    public float moveSpeed = 0.5f;
    public float lerpSpeed = 0.2f;
    Rigidbody2D rb;
    Transform target;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;
        else
            rb.velocity = Vector2.Lerp(rb.velocity, Vector3.MoveTowards(transform.position, target.position, moveSpeed) - transform.position, lerpSpeed);
    }

    private void OnCollisionStay2D(Collision2D collision)
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
