using System.Collections;
using System.Drawing;
using UnityEngine;

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
    private Vector3 camVel;
    private Rigidbody2D rb;
    private Transform camTransform;
    private Vector2 velocity;

    private void Start()
    {
        camTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //transfokrm.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x, transform.rotation.y, Mathf.Atan2(fixedJoystick.Vertical, fixedJoystick.Horizontal), transform.rotation.w), lerpSpeed);

        if (Input.GetKeyDown(KeyCode.Space))
            Dash();
    }

    private void LateUpdate()
    {
        camTransform.position = Vector3.SmoothDamp(camTransform.position, new Vector3(transform.position.x, transform.position.y, -10), ref camVel, lerpSpeed);
    }

    private void FixedUpdate()
    {
        //Vector3 direction = new Vector3(fixedJoystick.Horizontal, fixedJoystick.Vertical);
        //Quaternion toRotation = Quaternion.FromToRotation(transform.up, direction);
        //transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, lerpSpeed * Time.time);
        if (!dashing)
        {
            rb.AddForce(new Vector2(fixedJoystick.Horizontal, fixedJoystick.Vertical).normalized * movementSpeed);
            rb.velocity = velocity;
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
        //Death logic
    }

    public void Dash()
    {
        if(!dashCD)
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
        rb.AddForce(rb.velocity * dashForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        dashing = false;
        invincible = false;
        yield return new WaitForSeconds(1.5f);
        dashCD = false;
        yield break;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && dashing)
        {
            collision.gameObject.GetComponent<BasicEnemyAI>().TakeDamage(Damage);
        }
    }
}
