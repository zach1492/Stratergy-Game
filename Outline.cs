using UnityEngine;

public class Outline : MonoBehaviour
{
    public Color outlineColor = Color.black;
    public float outlineWidth = 0.03f;

    private Material outlineMaterial;

    void Awake()
    {
        outlineMaterial = new Material(Shader.Find("Custom/Outline"));
        outlineMaterial.SetColor("_OutlineColor", outlineColor);
        outlineMaterial.SetFloat("_OutlineWidth", outlineWidth);

        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mr in renderers)
        {
            var mats = mr.materials;
            System.Array.Resize(ref mats, mats.Length + 1);
            mats[mats.Length - 1] = outlineMaterial;
            mr.materials = mats;
        }
    }

    public void SetOutline(bool enabled)
    {
        outlineMaterial.SetFloat("_OutlineWidth", enabled ? outlineWidth : 0f);
    }

    public void SetColor(Color color)
    {
        outlineMaterial.SetColor("_OutlineColor", color);
    }
}
