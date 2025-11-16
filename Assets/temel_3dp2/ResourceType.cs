using UnityEngine;

[System.Serializable]
public class ResourceType
{
    public string name;
    public Sprite icon;
    public float amount;
    public bool canBeNegative;

    public ResourceType(string name, float amount = 0, bool canBeNegative = false)
    {
        this.name = name;
        this.amount = amount;
        this.canBeNegative = canBeNegative;
    }
}
