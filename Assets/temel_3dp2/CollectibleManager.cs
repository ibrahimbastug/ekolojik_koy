using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CollectibleType
{
    public string name;
    public GameObject prefab;
    public string resourceName; // örn: "Wood"
    public float amount = 5;
    public int spawnCount = 10;
}

public class CollectibleManager : MonoBehaviour
{
    public List<CollectibleType> collectibleTypes = new List<CollectibleType>();
    public Terrain terrain;
    public float offsetY = 0.1f;
    public GameManager gameManager;

    void Start()
    {
        if (!terrain)
        {
            terrain = Terrain.activeTerrain;
            if (!terrain)
            {
                Debug.LogError("CollectibleManager: Terrain bulunamadı!");
                return;
            }
        }

        if (!gameManager)
        {
            gameManager = FindObjectOfType<GameManager>();
            if (!gameManager)
            {
                Debug.LogError("CollectibleManager: GameManager bulunamadı!");
                return;
            }
        }

        SpawnAll();
    }

    void SpawnAll()
    {
        foreach (var type in collectibleTypes)
        {
            if (type.prefab == null)
            {
                Debug.LogWarning($"CollectibleManager: '{type.name}' prefabı atanmamış!");
                continue;
            }

            for (int i = 0; i < type.spawnCount; i++)
            {
                float randX = Random.Range(0f, terrain.terrainData.size.x);
                float randZ = Random.Range(0f, terrain.terrainData.size.z);
                float worldX = terrain.GetPosition().x + randX;
                float worldZ = terrain.GetPosition().z + randZ;
                float terrainY = terrain.SampleHeight(new Vector3(worldX, 0, worldZ)) + terrain.GetPosition().y;

                GameObject obj = Instantiate(type.prefab, new Vector3(worldX, terrainY + offsetY, worldZ), Quaternion.identity);
                obj.tag = "Toplanacak";

                var item = obj.AddComponent<CollectibleItem>();
                item.manager = this;
                item.resourceName = type.resourceName;
                item.amount = type.amount;



            }
        }
    }

    public void OnCollected(string resource, float amount)
    {
        if (gameManager)
            gameManager.Collect(resource, amount);
        else
            Debug.LogWarning("CollectibleManager: GameManager atanmamış!");
    }
}
