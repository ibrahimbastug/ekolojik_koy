using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public GameManager gameManager;
    public float buildDistance = 3f;

    public void Build(BuildingType building)
    {
        if (!gameManager.HasEnoughResources(building.effects))
        {
            Debug.Log("Kaynak yetersiz!");
            return;
        }

        Vector3 buildPos = Camera.main.transform.position + Camera.main.transform.forward * buildDistance;

        if (Physics.Raycast(buildPos + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 50f))
            buildPos.y = hit.point.y;

        Instantiate(building.prefab, buildPos, Quaternion.identity);
        gameManager.ApplyEffects(building.effects);

        Debug.Log($"{building.name} inşa edildi!");
    }
}
