public class Item
{
    public Item(string name, string description, float damage = 0, float health = 0, float speed = 0)
    {
        Name = name;
        Description = description;
        Damage = damage;
        Health = health;
        Speed = speed;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public int Stack { get; set; } = 1;
    public float Damage { get; set; }
    public float Health { get; set; }
    public float Speed { get; set; }
}