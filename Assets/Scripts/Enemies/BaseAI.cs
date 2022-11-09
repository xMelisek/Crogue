using UnityEngine;

public abstract class BaseAI : MonoBehaviour
{
    public abstract void TakeDamage(float dmgVal);

    public abstract void Die();
}
