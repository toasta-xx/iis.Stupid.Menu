using UnityEngine;
using UnityEngine.Rendering;

namespace iiMenu.Utilities
{
    public static class MaterialUtilities
    {
        public static Material CreateTransparentUnlitMaterial(Texture2D texture = null)
        {
            var mat = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            if (texture != null) mat.mainTexture = texture;
            mat.SetFloat("_Surface", 1);
            mat.SetFloat("_Blend", 0);
            mat.SetFloat("_SrcBlend", (float)BlendMode.SrcAlpha);
            mat.SetFloat("_DstBlend", (float)BlendMode.OneMinusSrcAlpha);
            mat.SetFloat("_ZWrite", 0);
            mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.renderQueue = (int)RenderQueue.Transparent;
            return mat;
        }
    }
}
