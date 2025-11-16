using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildMenuUI : MonoBehaviour
{
    public GameManager gameManager;
    public BuildManager buildManager;

    [Header("UI")]
    public Transform buttonContainer;  // VerticalLayoutGroup içeren GameObject
    public GameObject buttonPrefab;    // Ýçinde Button + TMP Text olan prefab

    void Start()
    {
        CreateButtons();
    }

    void CreateButtons()
    {
        foreach (var building in gameManager.buildingTypes)
        {
            BuildingType b = building; // closure fix

            GameObject btnObj = Instantiate(buttonPrefab, buttonContainer);
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = b.name;

            Button btn = btnObj.GetComponent<Button>();
            btn.onClick.AddListener(() => buildManager.Build(b));
        }
    }
}
