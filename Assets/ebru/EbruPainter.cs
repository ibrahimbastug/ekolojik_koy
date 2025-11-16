using UnityEngine;
using System.Collections.Generic;

public class EbruPainter : MonoBehaviour
{
    public Camera mainCamera;
    public RenderTexture targetTexture;
    public Color brushColor = Color.red;
    public float dropRadius = 0.02f;
    public float spreadSpeed = 0.2f;
    public float fadeSpeed = 0.3f;

    private Material paintMaterial;

    struct Drop
    {
        public Vector2 uv;
        public Color color;
        public float startTime;
    }

    private List<Drop> drops = new List<Drop>();

    void Start()
    {
        paintMaterial = new Material(Shader.Find("Hidden/EbruDrop"));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                drops.Add(new Drop
                {
                    uv = hit.textureCoord,
                    color = brushColor,
                    startTime = Time.time
                });
            }
        }

        if (drops.Count == 0) return;

        Graphics.SetRenderTarget(targetTexture);
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, targetTexture.width, targetTexture.height, 0);

        paintMaterial.SetPass(0);

        GL.Begin(GL.QUADS);

        foreach (var d in drops)
        {
            float age = Time.time - d.startTime;
            if (age > 5f) continue;

            float radius = dropRadius + age * spreadSpeed;
            float alpha = Mathf.Lerp(1f, 0f, age * fadeSpeed);
            paintMaterial.SetColor("_Color", new Color(d.color.r, d.color.g, d.color.b, alpha));
            paintMaterial.SetVector("_Center", new Vector4(d.uv.x, d.uv.y, 0, 0));
            paintMaterial.SetFloat("_Radius", radius);

            float px = d.uv.x * targetTexture.width;
            float py = (1 - d.uv.y) * targetTexture.height;
            float size = radius * targetTexture.width;

            GL.Vertex3(px - size / 2f, py - size / 2f, 0);
            GL.Vertex3(px + size / 2f, py - size / 2f, 0);
            GL.Vertex3(px + size / 2f, py + size / 2f, 0);
            GL.Vertex3(px - size / 2f, py + size / 2f, 0);
        }

        GL.End();
        GL.PopMatrix();
        Graphics.SetRenderTarget(null);
    }
}
