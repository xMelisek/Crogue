using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [Tooltip("Bottom left point of the arena")]
    public Vector3 blp;
    [Tooltip("Top right point of the arena")]
    public Vector3 trp;
    public float minSpawnDist;
    public GameObject[] enemies;
    public int maxEntityCount = 20;

    private void FixedUpdate()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length < maxEntityCount)
        {
            if (Random.Range(1, (int)Mathf.Clamp(50 - Time.time / 100f, 5f, 50f)) == 1)
            {
                for (int i = 0; i < 100; i++)
                {
                    Vector3 spawnpos = new Vector3(Random.Range(blp.x, trp.x), Random.Range(blp.y, trp.y), 0);
                    if (Vector3.Distance(Camera.main.transform.position, spawnpos) < minSpawnDist)
                        continue;
                    Instantiate(enemies[Random.Range(0, enemies.Length)], spawnpos, Quaternion.identity);
                    break;
                }
            }
        }
    }
}
