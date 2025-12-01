using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("KAYNAKLAR")]
    public List<ResourceEntry> resources = new List<ResourceEntry>();

    [Header("BİNA VE NESNELER")]
    public List<BuildingType> buildingTypes = new List<BuildingType>();

    [Header("UI")]
    public TextMeshProUGUI resourceText;

    void Start()
    {
        UpdateResourceUI();
    }

    // Belirli bir kaynağın kaydını bul
    public ResourceEntry GetEntry(ResourceType type)
    {
        if (type == null) return null;
        return resources.Find(r => r != null && r.type == type);
    }

    public void ChangeResource(ResourceType type, float delta)
    {
        if (type == null)
        {
            Debug.LogWarning("ChangeResource: ResourceType null!");
            return;
        }

        var entry = GetEntry(type);
        if (entry == null)
        {
            Debug.LogWarning($"ChangeResource: {type.resourceName} için ResourceEntry bulunamadı!");
            return;
        }

        entry.amount += delta;

        if (!type.canBeNegative && entry.amount < 0)
            entry.amount = 0;

        UpdateResourceUI();
    }

    public bool HasEnoughResources(ResourceEffect[] effects)
    {
        if (effects == null) return true;

        foreach (var eff in effects)
        {
            if (eff == null || eff.resource == null) continue;

            var entry = GetEntry(eff.resource);
            if (entry == null)
            {
                // Bu kaynağı hiç tanımlamamışsak, yeterli değil say
                return false;
            }

            float newAmount = entry.amount + eff.changeAmount;

            // changeAmount genelde negatif (maliyet). Yeni miktar 0'ın altına düşüyorsa ve negatif izinli değilse: yetersiz.
            if (!eff.resource.canBeNegative && newAmount < 0)
                return false;
        }

        return true;
    }

    public void ApplyEffects(ResourceEffect[] effects)
    {
        if (effects == null) return;

        foreach (var eff in effects)
        {
            if (eff == null || eff.resource == null) continue;
            ChangeResource(eff.resource, eff.changeAmount);
        }
    }

    public void Collect(ResourceType resource, float amount)
    {
        if (resource == null) return;

        ChangeResource(resource, amount);
        Debug.Log($"{resource.resourceName} kaynağından {amount} toplandı!");
    }

    void UpdateResourceUI()
    {
        if (resourceText == null) return;

        List<string> parts = new List<string>();
        foreach (var entry in resources)
        {
            if (entry == null || entry.type == null) continue;
            parts.Add($"{entry.type.resourceName}: {entry.amount}");
        }

        resourceText.text = string.Join(" | ", parts);
    }
}

[System.Serializable]
public class ResourceEntry
{
    public ResourceType type;
    public float amount;
}
