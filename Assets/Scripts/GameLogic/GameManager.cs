using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Vector3 blp;
    public Vector3 trp;
    public GameObject itemPedestal;
    public float itemSpawnChance = 0.1f;

    void FixedUpdate()
    {
        if (GameObject.FindGameObjectsWithTag("Item").Length < 1)
        {
            if (Random.Range(0.01f, 1f) < itemSpawnChance)
            {
                Instantiate(itemPedestal, new Vector2(Random.Range(blp.x, trp.x), Random.Range(blp.y, trp.y)), Quaternion.identity);
            }
        }
    }
}
