using UnityEngine;

[ExecuteAlways]
public class EbruWave : MonoBehaviour
{
    public Material waterMaterial;
    public RenderTexture ebruTexture;
    private RenderTexture tempTexture;

    void Start()
    {
        // Eðer Inspector'dan material verilmediyse, otomatik oluþtur:
        if (waterMaterial == null)
            waterMaterial = new Material(Shader.Find("Hidden/EbruWater"));

        tempTexture = new RenderTexture(ebruTexture.width, ebruTexture.height, 0);
    }

    void Update()
    {
        if (waterMaterial == null || ebruTexture == null) return;

        // EbruTexture’ý su efektiyle sürekli güncelle
        Graphics.Blit(ebruTexture, tempTexture, waterMaterial);
        Graphics.Blit(tempTexture, ebruTexture);
    }

    void OnDestroy()
    {
        if (tempTexture != null) tempTexture.Release();
    }
}
