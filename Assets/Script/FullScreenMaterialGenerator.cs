using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenMaterialGenerator {

    private Material _mat;
    private Texture _tex;

    public void SetTexture(Texture tex)
    {
        var mat = GetMaterial();
        mat.SetTexture("_MainTex", tex);
    }

    private void RefreshTexture()
    {
        if(_mat != null && _tex == null)
        {
            var root = GameObject.Find("RayTracingRendererRoot");
            var renderer = root.GetComponent<RayTracingRenderer>();
            _tex = renderer.Tex;
            SetTexture(_tex);
        }
    }

    public Material GetMaterial()
    {
        if(_mat == null)
        {
            _mat = new Material(Shader.Find("Unlit/FullScreenPlaneShader"));
        }

        RefreshTexture();

        return _mat;
    }
}
