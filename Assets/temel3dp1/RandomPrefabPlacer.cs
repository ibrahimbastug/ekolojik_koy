using UnityEngine;
using System.Collections.Generic;

public class RandomPrefabPlacer : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] prefabs;

    [Header("Placement Settings")]
    public int spawnCount = 20;                 // toplam yerleştirilecek adet
    public float areaRadius = 50f;              // merkezden yarıçap (düzleştirilmiş alan)
    public LayerMask groundLayer = ~0;          // zemin layer'ı (Terrain collider veya ground layer belirtin)
    public float minY = -100f;                  // raycast başlangıç Y'si
    public float maxY = 200f;                   // raycast başlangıç Y'si
    public float minDistanceBetween = 1.5f;     // objeler arası minimum uzaklık (çakışmayı azaltmak için)
    public int maxPlacementAttempts = 20;       // tek bir obje için denenebilecek maksimum pozisyon denemesi

    [Header("Align & Randomize")]
    public bool alignToNormal = true;           // zeminin normaliyle hizala mı
    public bool randomYRotation = true;         // y ekseninde rastgele dönsün mü
    public Vector2 scaleRange = Vector2.one;    // min ve max scale (ör: (0.8,1.2))

    [Header("Parenting & Debug")]
    public Transform parentForSpawned;          // spawned objeleri buraya parent et (opsiyonel)
    public bool drawDebugRays = false;

    // Internal
    private List<Vector3> placedPositions = new List<Vector3>();

    void Start()
    {
        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogWarning("Prefabs array is empty. Please assign prefabs in the inspector.");
            return;
        }

        PlaceRandomPrefabs();
    }

    public void PlaceRandomPrefabs()
    {
        placedPositions.Clear();

        for (int i = 0; i < spawnCount; i++)
        {
            bool placed = false;

            for (int attempt = 0; attempt < maxPlacementAttempts; attempt++)
            {
                // rastgele horizontal nokta (bu objeyi spawn edilecek merkeze göre)
                Vector2 randCircle = Random.insideUnitCircle * areaRadius;
                Vector3 probePos = transform.position + new Vector3(randCircle.x, maxY, randCircle.y);

                // Raycast aşağı
                Ray ray = new Ray(probePos, Vector3.down);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, maxY - minY + Mathf.Abs(maxY), groundLayer))
                {
                    if (drawDebugRays) Debug.DrawLine(probePos, hit.point, Color.green, 5f);

                    // çakışma kontrolü: yakınlığa bak
                    bool tooClose = false;
                    foreach (var p in placedPositions)
                    {
                        if (Vector3.Distance(p, hit.point) < minDistanceBetween)
                        {
                            tooClose = true;
                            break;
                        }
                    }
                    if (tooClose) continue;

                    // seçilen prefab
                    GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
                    if (prefab == null) continue;

                    // instantiate
                    Quaternion rot = Quaternion.identity;
                    if (alignToNormal)
                    {
                        // zeminin normaliyle hizala (sadece y döndürmesi değil, tam yönlendirme)
                        rot = Quaternion.FromToRotation(Vector3.up, hit.normal);
                        if (randomYRotation)
                        {
                            // normali bozmadan local Y ekseninde rastgele dön
                            rot *= Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                        }
                    }
                    else if (randomYRotation)
                    {
                        rot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                    }

                    GameObject spawned = Instantiate(prefab, hit.point, rot, parentForSpawned);
                    // rastgele scale uygula
                    float s = Random.Range(scaleRange.x, scaleRange.y);
                    spawned.transform.localScale *= s;

                    // eğer prefab kendisi physics collider gerektiriyorsa, spawn noktasını biraz yukarı taşı (opsiyonel)
                    // spawned.transform.position += Vector3.up * 0.01f;

                    placedPositions.Add(hit.point);
                    placed = true;
                    break; // attempt döngüsünden çık, sonraki objeye geç
                }
                else
                {
                    if (drawDebugRays) Debug.DrawLine(probePos, probePos + Vector3.down * (maxY - minY + Mathf.Abs(maxY)), Color.red, 2f);
                }
            } // attempt sonu

            if (!placed)
            {
                Debug.LogWarning($"Prefab #{i} için uygun pozisyon bulunamadı (max attempts: {maxPlacementAttempts}).");
            }
        } // spawnCount sonu
    }

    // Optional: editör sırasında görsel alan
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, areaRadius);
    }
}
