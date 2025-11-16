using UnityEngine;
using UnityEngine.UI;

public class ObjectCollector : MonoBehaviour
{
    [Header("UI Ayarları")]
    public Text counterText;          // Canvas üzerindeki Text
    public float collectDistance = 2f; // Maksimum toplama mesafesi

    private int collectedCount = 0;    // Toplanan nesne sayısı

    void Start()
    {
        UpdateCounter();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 2 birimlik mesafeye kadar raycast yap
            if (Physics.Raycast(ray, out hit, collectDistance))
            {
                GameObject obj = hit.collider.gameObject;

                // Sadece "Toplanacak" tag'li nesneleri topla
                if (obj.CompareTag("Toplanacak"))
                {
                    Destroy(obj);
                    collectedCount++;
                    UpdateCounter();
                }
            }
        }
    }

    void UpdateCounter()
    {
        if (counterText != null)
        {
            counterText.text = "Toplanan Nesne: " + collectedCount;
        }
    }
}
