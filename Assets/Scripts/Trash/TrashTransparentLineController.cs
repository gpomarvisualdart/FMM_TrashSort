using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashTransparentLineController : MonoBehaviour
{
    Renderer rend;


    private void OnEnable()
    {
        Invoke("TurnOff", 0.3f);
    }


    public void InitializeMaterial(string color)
    {
        if (rend == null) rend = GetComponent<Renderer>();
        if (!rend.material.name.EndsWith("(Instance)"))
        {
            rend.material = new Material(rend.material);
        }

        Color clr;
        if (ColorUtility.TryParseHtmlString(color, out clr))
        {
            clr.a = 0.2f;
        }

        Material mat = rend.material;
        mat.SetFloat("_Surface", 1); // Transparent
        mat.SetFloat("_Blend", 0);   // 0 = Alpha blend
        mat.SetFloat("_ZWrite", 0);  // Turn off ZWrite for transparency
        mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

        mat.SetColor("_BaseColor", clr);
    }


    private void TurnOff()
    {
        gameObject.SetActive(false);
    }
}
