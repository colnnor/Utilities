using UnityEngine;
using UnityEngine.Rendering;

public static class Materials
{
    private static readonly int surface = Shader.PropertyToID("_Surface");
    private static readonly int blend = Shader.PropertyToID("_Blend");
    private static readonly int dstBlend = Shader.PropertyToID("_DstBlend");
    private static readonly int zWrite = Shader.PropertyToID("_ZWrite");
    private static readonly int alphaClip = Shader.PropertyToID("_AlphaClip");

    public static Material GetDefault()
    {
        if(Camera.main != null)
        {
            var rp = UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline;
            if (rp != null)
            {
                var rpType = rp.GetType().ToString();
                if (rpType.Contains("UniversalRenderPipelineAsset"))
                {
                    return GetURPLit();
                }
                else if (rpType.Contains("HDRenderPipelineAsset"))
                {
                    return GetHDRPLit();
                }
                else
                {
                    return GetStandard();
                }
            }
        }

        return GetURPLit();
    }
    public static Material GetStandard(bool transparent = false, Color? color = null)
    {
        Color setColor = color ?? Color.gray;
        
        Material mat = new Material(Shader.Find("Standard"))
        {
            color = Color.gray.WithAlpha(0f)
        };

        if (transparent)
        {
            mat.SetFloat("_Mode", 3); // 0 = Opaque, 1 = Cutout, 2 = Fade, 3 = Transparent
            mat.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0); // Disable depth writing
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = (int)RenderQueue.Transparent; // Set to Transparent Queue
        }
        else
        {
            mat.SetFloat("_Mode", 0); // Opaque
            mat.SetInt("_SrcBlend", (int)BlendMode.One);
            mat.SetInt("_DstBlend", (int)BlendMode.Zero);
            mat.SetInt("_ZWrite", 1); // Enable depth writing
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.DisableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = (int)RenderQueue.Geometry; // Set to Geometry Queue
        }

        mat.color = setColor;
        return mat;
    }
    public static Material GetHDRPLit(bool transparent = false, Color? color = null)
    {
        Color setColor = color ?? Color.gray;
        
        Material mat = new Material(Shader.Find("HDRP/Lit"))
        {
            color = Color.gray.WithAlpha(0f)
        };

        if (transparent)
        {
            mat.SetFloat("_SurfaceType", 1); // 0 = Opaque, 1 = Transparent
            mat.SetFloat("_BlendMode", 0); // Alpha blending
            mat.SetFloat("_DstBlend", (float)BlendMode.OneMinusSrcAlpha);
            mat.SetFloat("_ZWrite", 0); // Disable depth writing
            mat.renderQueue = (int)RenderQueue.Transparent; // Set to Transparent Queue

            // Enable these keywords for HDRP transparency to work properly
            mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.EnableKeyword("_BLENDMODE_ALPHA");
        }
        else
        {
            mat.SetFloat("_SurfaceType", 0); // Opaque
            mat.SetFloat("_ZWrite", 1); // Enable depth writing
            mat.renderQueue = (int)RenderQueue.Geometry; // Set to Geometry Queue

            // Disable transparency keywords
            mat.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.DisableKeyword("_BLENDMODE_ALPHA");
        }

        mat.color = setColor;
        return mat;
    }
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