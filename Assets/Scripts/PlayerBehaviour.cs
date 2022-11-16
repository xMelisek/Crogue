using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    public float Health { get; private set; } = 100f;
    public float Damage { get; set; } = 10f;
    [SerializeField] private FixedJoystick fixedJoystick;
    public float movementSpeed = 5f;
    public float lerpSpeed = 0.2f;
    public float dashForce = 100f;
    public float iTime = 1f;
    public bool dashCD = false;
    public bool dashing = false;
    public bool invincible = false;
    public GameObject deathPart;
    private Vector3 camVel;
    private Rigidbody2D rb;
    private Transform camTransform;
    private BoxCollider2D bCollider;

    private void Start()
    {
        camTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody2D>();
        bCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        transform.up = Vector3.Lerp(transform.up, new Vector3(fixedJoystick.Horizontal, fixedJoystick.Vertical), lerpSpeed);
        if (Input.GetKeyDown(KeyCode.Space))
            Dash();
    }

    private void FixedUpdate()
    {
        camTransform.position = Vector3.SmoothDamp(camTransform.position, new Vector3(transform.position.x, transform.position.y, -10), ref camVel, lerpSpeed);

        if (!dashing)
        {
            rb.velocity = new Vector2(fixedJoystick.Horizontal, fixedJoystick.Vertical).normalized * movementSpeed;
        }
    }

    public void TakeDamage(float damage)
    {
        if (invincible) return;

        StartCoroutine(Invincible(iTime));
        Health -= damage;
        if (Health <= 0) Die();
    }

    public void Die()
    {
        ParticleBehaviour partBehaviour = Instantiate(deathPart, transform.position, Quaternion.identity).GetComponent<ParticleBehaviour>();
        partBehaviour.StartCoroutine(partBehaviour.KillAfter(3f));
        var uIScript = FindObjectOfType<UIScript>();
        uIScript.HUD = false;
        uIScript.StartCoroutine(uIScript.ToggleDeathUI());
        Destroy(gameObject);
    }

    public void Dash()
    {
        if (!dashCD)
            StartCoroutine(DashCoroutine());
    }

    IEnumerator Invincible(float time)
    {
        invincible = true;
        yield return new WaitForSeconds(time);
        invincible = false;
        yield break;
    }

    IEnumerator DashCoroutine()
    {
        dashCD = true;
        dashing = true;
        invincible = true;
        bCollider.isTrigger = true;
        rb.AddForce(rb.velocity * dashForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        dashing = false;
        invincible = false;
        bCollider.isTrigger = false;
        yield return new WaitForSeconds(1.5f);
        dashCD = false;
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && dashing)
        {
            collision.gameObject.GetComponent<BasicEnemyAI>().TakeDamage(Damage);
        }
        else if (collision.gameObject.CompareTag("Border") && dashing)
        {
            dashing = false;
            bCollider.isTrigger = false;
        }
    }
}
