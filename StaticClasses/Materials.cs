using UnityEngine;
using UnityEngine.Rendering;

public static class Materials
{
    private static readonly int surface = Shader.PropertyToID("_Surface");
    private static readonly int blend = Shader.PropertyToID("_Blend");
    private static readonly int dstBlend = Shader.PropertyToID("_DstBlend");
    private static readonly int zWrite = Shader.PropertyToID("_ZWrite");
    private static readonly int alphaClip = Shader.PropertyToID("_AlphaClip");

    public static Material GetURPLit(bool transparent = false, Color? color = null)
    {
        Color setColor = color ?? Color.gray;
        
        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"))
        {
            color = Color.gray.WithAlpha(0f)
        };

        mat.SetFloat(surface, transparent ? 1 : 0); // 0 = Opaque, 1 = Transparent
        mat.SetFloat(blend, (float)BlendMode.SrcAlpha); // Set blend mode
        mat.SetFloat(dstBlend, (float)BlendMode.OneMinusSrcAlpha); // Alpha blending
        mat.SetFloat(zWrite, 0); // Disable depth writing
        mat.renderQueue = (int)RenderQueue.Transparent; // Set to Transparent Queue

        // Enable Alpha Clipping (Optional, for cutout transparency)

        mat.SetFloat(alphaClip, 0); // 1 = Clipping, 0 = No Clipping

        // Enable these keywords for URP transparency to work properly
        mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        mat.EnableKeyword("_BLENDMODE_ALPHA");

        mat.color = setColor;
        return mat;
    }
}