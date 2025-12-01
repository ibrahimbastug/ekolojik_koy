using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public CollectibleManager manager;
    public ResourceType resourceName;
    public float amount = 5f;
    public float collectDistance = 3f;

    void Start()
    {
        // Eðer prefab'ta collider yoksa ekle
        if (GetComponent<Collider>() == null)
        {
            var col = gameObject.AddComponent<BoxCollider>();
            col.isTrigger = true; // Trigger olsun, çarpýþma engellemesin
        }

        // Tag yoksa "Toplanacak" yap
        if (gameObject.CompareTag("Untagged"))
        {
            gameObject.tag = "Toplanacak";
        }
    }

    void Update()
    {
        // Manager atanmadýysa, toplanamaz
        if (manager == null)
            return;

        // SOL MOUSE TIKLAMASI
        if (Input.GetMouseButtonDown(0))
        {
            // Mouse pozisyonundan ray at
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                // Çarpýlan obje bu mu ve tag "Toplanacak" mý
                if (hit.collider.gameObject == gameObject && CompareTag("Toplanacak"))
                {
                    float dist = Vector3.Distance(Camera.main.transform.position, transform.position);

                    if (dist <= collectDistance)
                    {
                        if (!resourceName)
                        {
                            Debug.LogWarning($"{name}: resourceName boþ, GameManager tarafýnda hiçbir kaynak artmayacak.");
                        }

                        manager.OnCollected(resourceName, amount);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
