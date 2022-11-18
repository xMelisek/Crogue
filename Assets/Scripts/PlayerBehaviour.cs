using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public readonly float baseHealth = 100f;
    public float Health { get; private set; }
    public float MaxHealth { get; private set; }
    private float Damage { get; set; } = 10f;
    public List<Item> Items { get; private set; }
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

    public event Action<float[]> OnHealthUpdate;

    #region Unity methods
    private void Start()
    {
        Health = baseHealth;
        MaxHealth = baseHealth;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && dashing)
        {
            collision.gameObject.GetComponent<BasicEnemyAI>().TakeDamage(GetDamage());
        }
        else if (collision.gameObject.CompareTag("Border") && dashing)
        {
            dashing = false;
            bCollider.isTrigger = false;
        }
    }
    #endregion

    public float GetDamage(bool raw = false)
    {
        if (raw) return Damage;
        float additionalDamage = 0;
        foreach (var item in Items)
            additionalDamage += item.Damage;
        return Damage + additionalDamage;
    }

    public void AddItem(Item item)
    {
        Items.Add(item);
        //Update item HUD or something like that if i add one
    }

    /// <summary>
    /// Deal damage to the player
    /// </summary>
    /// <param name="damage">Damage to be dealt</param>
    /// <returns>If the damage was lethal or not</returns>
    public bool TakeDamage(float damage)
    {
        if (invincible) return false;

        StartCoroutine(Invincible(iTime));
        Health -= damage;
        OnHealthUpdate.Invoke(new float[2] { Health, MaxHealth });
        if (Health <= 0) 
        { 
            Die();
            return true;
        }
        return false;
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
}
