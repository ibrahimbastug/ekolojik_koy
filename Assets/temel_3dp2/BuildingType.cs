using UnityEngine;

[System.Serializable]
public class BuildingType
{
    public string name;           // Örn: "Ev", "Kömür Santrali", "Ağaç"
    public GameObject prefab;     // Sahneye yerleştirilecek nesne
    public bool isCollectible;    // Toplanabilir mi?

    [Header("Maliyet veya Üretim")]
    public ResourceEffect[] effects; // Toplanınca veya inşa edilince kaynak değişimleri
}

[System.Serializable]
public class ResourceEffect
{
    public string resourceName;   // Örn: "Wood", "Pollution"
    public float changeAmount;    // + veya - etki miktarı
}
