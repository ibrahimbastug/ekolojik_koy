using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public CollectibleManager manager;
    public string resourceName;
    public float amount = 5f;
    public float collectDistance = 3f;

    void Start()
    {
        // Eðer prefab'ta collider yoksa ekle
        if (GetComponent<Collider>() == null)
        {
            var col = gameObject.AddComponent<BoxCollider>();
            col.isTrigger = true; // Trigger olmalý ki fizik engellemesin
        }

        // Tag'ý kontrol et
        if (!CompareTag("Toplanacak"))
            gameObject.tag = "Toplanacak";
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                // Artýk collider trigger olsa bile çarpacak
                if (hit.collider.gameObject == gameObject && CompareTag("Toplanacak"))
                {
                    float dist = Vector3.Distance(Camera.main.transform.position, transform.position);
                    if (dist <= collectDistance)
                    {
                        manager.OnCollected(resourceName, amount);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
