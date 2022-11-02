using UnityEngine;

public class LevelGen : MonoBehaviour
{
    [SerializeField] private Vector3 startPoint;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float xInterval;
    [SerializeField] private float yInterval;
    public float heightMultiplier;
    [Range(0.01f, 1000)] public float scale = 1f;
    public bool genOnSpace;
    [SerializeField] Layer[] layers;

    System.Random prng;

    void Start()
    {
        GenerateTerrain();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && genOnSpace) GenerateTerrain();
    }

    public void GenerateTerrain()
    {
        prng = new System.Random(Random.Range(-100000, 100000));
        float xOrg = prng.Next(-100000, 100000);
        float yOrg = prng.Next(-100000, 100000);
        GenerateTerrain(CalcMap(width, height, xOrg, yOrg));
    }

    float[,] CalcMap(int width, int height, float xOrg, float yOrg)
    {

        float[,] noiseMap = new float[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float xCoord = xOrg + (float)x / width * scale;
                float yCoord = yOrg + (float)y / height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord) * heightMultiplier;
                noiseMap[x, y] = sample;
            }
        }

        return noiseMap;
    }

    void GenerateTerrain(float[,] noiseMap)
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        for (int y = 0; y < noiseMap.GetLength(1); y++)
        {
            for (int x = 0; x < noiseMap.GetLength(0); x++)
            {
                foreach (var layer in layers)
                {
                    if (noiseMap[x, y] < layer.height)
                    {
                        var prefab = Instantiate(layer.prefab, new Vector3(startPoint.x + x * xInterval, startPoint.y + y * yInterval, startPoint.z), Quaternion.identity);
                        prefab.transform.SetParent(transform, true);
                        break;
                    }
                }
            }
        }
    }
}

[System.Serializable]
public struct Layer
{
    public GameObject prefab;
    public float height;

    public Layer(GameObject prefab, float height)
    {
        this.prefab = prefab;
        this.height = height;
    }
}