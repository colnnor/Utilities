using UnityEngine;
using UnityEngine.Rendering;

public static class Materials
{
    private static readonly int surface = Shader.PropertyToID("_Surface");
    private static readonly int blend = Shader.PropertyToID("_Blend");
    private static readonly int dstBlend = Shader.PropertyToID("_DstBlend");
    private static readonly int zWrite = Shader.PropertyToID("_ZWrite");
    private static readonly int alphaClip = Shader.PropertyToID("_AlphaClip");

    public static Material Default(bool transparent = false, Color? color = null)
    {
        #if UNITY_URP
        return GetURPLit(transparent, color);
        #elif UNITY_HDRP
        return GetHDRPLit(transparent, color);
        #else
        return GetStandard(transparent, color);
        #endif
    }
    public static Material GetURPLit(bool transparent = false, Color? color = null)
    {
        Color setColor = color ?? Color.gray;
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (!shader) return GetStandard(transparent, setColor);

        Material mat = new Material(shader)
        {
            color = setColor.WithAlpha(0f)
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

    public static Material GetHDRPLit(bool transparent = false, Color? color = null)
    {
        Color setColor = color ?? Color.gray;
        Shader shader = Shader.Find("HDRP/Lit");
        if (!shader) return GetStandard(transparent, setColor);

        Material mat = new Material(shader)
        {
            color = setColor.WithAlpha(0f)
        };

        mat.SetFloat("_SurfaceType", transparent ? 1f : 0f);
        mat.SetFloat("_BlendMode", 0f);
        mat.SetFloat("_SrcBlend", (float)BlendMode.SrcAlpha);
        mat.SetFloat("_DstBlend", (float)BlendMode.OneMinusSrcAlpha);
        mat.SetFloat("_ZWrite", transparent ? 0f : 1f);
        mat.SetFloat("_AlphaCutoffEnable", 0f);
        mat.SetFloat("_CullMode", 2f);
        mat.renderQueue = transparent ? (int)RenderQueue.Transparent : (int)RenderQueue.Geometry;

        if (transparent)
        {
            mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.EnableKeyword("_BLENDMODE_ALPHA");
            mat.EnableKeyword("_ENABLE_FOG_ON_TRANSPARENT");
        }
        else
        {
            mat.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.DisableKeyword("_BLENDMODE_ALPHA");
            mat.DisableKeyword("_ENABLE_FOG_ON_TRANSPARENT");
        }

        mat.color = setColor;
        return mat;
    }

    public static Material GetStandard(bool transparent = false, Color? color = null)
    {
        Color setColor = color ?? Color.gray;
        Shader shader = Shader.Find("Standard") ?? Shader.Find("Sprites/Default");
        if (!shader) return null;

        Material mat = new Material(shader)
        {
            color = setColor
        };

        if (transparent)
        {
            mat.SetFloat("_Mode", 3f);
            mat.SetFloat("_SrcBlend", (float)BlendMode.SrcAlpha);
            mat.SetFloat("_DstBlend", (float)BlendMode.OneMinusSrcAlpha);
            mat.SetFloat("_ZWrite", 0f);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = (int)RenderQueue.Transparent;
        }
        else
        {
            mat.SetFloat("_Mode", 0f);
            mat.SetFloat("_SrcBlend", (float)BlendMode.One);
            mat.SetFloat("_DstBlend", (float)BlendMode.Zero);
            mat.SetFloat("_ZWrite", 1f);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.DisableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = (int)RenderQueue.Geometry;
        }

        mat.color = setColor;
        return mat;
    }
}