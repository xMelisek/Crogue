using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int stack { get; private set; }
}