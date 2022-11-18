using UnityEngine;

public abstract class BaseAI : MonoBehaviour
{
    /// <summary>
    /// Deals damage to the entity
    /// </summary>
    /// <param name="dmgVal">Damage dealt</param>
    /// <returns>If the damage was lethal or not</returns>
    public abstract bool TakeDamage(float dmgVal);

    public abstract void Die();
}
