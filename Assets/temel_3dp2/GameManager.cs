using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("KAYNAKLAR")]
    public List<ResourceType> resources = new List<ResourceType>();

    [Header("BİNA VE NESNELER")]
    public List<BuildingType> buildingTypes = new List<BuildingType>();

    [Header("UI")]
    public TextMeshProUGUI resourceText;

    void Start()
    {
        InitializeDefaultResources();
        UpdateUI();
    }

    void InitializeDefaultResources()
    {
        if (resources.Count == 0)
        {
            resources.Add(new ResourceType("Wood", 0));
            resources.Add(new ResourceType("Stone", 0));
            resources.Add(new ResourceType("Energy", 0));
            resources.Add(new ResourceType("Pollution", 0, true));
        }
    }

    public void UpdateUI()
    {
        string text = "";
        foreach (var res in resources)
        {
            text += $"{res.name}: {res.amount}\n";
        }
        resourceText.text = text;
    }

    public void ChangeResource(string name, float amount)
    {
        ResourceType res = resources.Find(r => r.name == name);
        if (res != null)
        {
            res.amount += amount;
            if (!res.canBeNegative && res.amount < 0)
                res.amount = 0;
        }
        UpdateUI();
    }

    public bool HasEnoughResources(ResourceEffect[] costs)
    {
        foreach (var cost in costs)
        {
            ResourceType res = resources.Find(r => r.name == cost.resourceName);
            if (res == null || res.amount + cost.changeAmount < 0)
                return false;
        }
        return true;
    }

    public void ApplyEffects(ResourceEffect[] effects)
    {
        foreach (var eff in effects)
        {
            ChangeResource(eff.resourceName, eff.changeAmount);
        }
    }

    public void Collect(string resource, float amount)
    {
        ChangeResource(resource, amount);
        Debug.Log($"{resource} kaynağından {amount} toplandı!");
    }
}
