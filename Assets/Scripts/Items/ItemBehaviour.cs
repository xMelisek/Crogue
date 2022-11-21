using Crogue.Core;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{
    public Item item;
    private Sprite[] sprites;

    private void Start()
    {
        sprites = Resources.LoadAll<Sprite>("Sprites/Items");
        int id = Random.Range(0, sprites.Length - 1);
        item = ItemID.GetItem(id);
        GetComponent<SpriteRenderer>().sprite = sprites[id];
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<PlayerBehaviour>().AddItem(item);
            Destroy(gameObject);
        }
    }
}