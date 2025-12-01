using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CollectibleType
{
    public string name;
    public GameObject prefab;
    public ResourceType resource;  // Örn: Wood
    public float amount = 5;
    public int spawnCount = 10;
}

public class CollectibleManager : MonoBehaviour
{
    public List<CollectibleType> collectibleTypes = new List<CollectibleType>();

    [Header("Yerleşim")]
    public float offsetY = 0.5f;   // Terrain yüzeyinden ne kadar yukarıda olsun?

    [Header("Referanslar")]
    public GameManager gameManager;

    void Start()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();

        Terrain terrain = FindObjectOfType<Terrain>();
        if (terrain == null)
        {
            Debug.LogWarning("CollectibleManager: Sahne üzerinde Terrain bulunamadı.");
            return;
        }

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

                GameObject obj = Instantiate(type.prefab,
                    new Vector3(worldX, terrainY + offsetY, worldZ),
                    Quaternion.identity);

                var item = obj.GetComponent<CollectibleItem>();
                if (item == null)
                    item = obj.AddComponent<CollectibleItem>();

                item.manager = this;
                item.resourceName = type.resource;
                item.amount = type.amount;
            }
        }
    }

    public void OnCollected(ResourceType resource, float amount)
    {
        if (gameManager)
            gameManager.Collect(resource, amount);
        else
            Debug.LogWarning("CollectibleManager: GameManager atanmamış!");
    }
}
