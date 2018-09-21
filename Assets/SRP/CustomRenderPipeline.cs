using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

public class CustomRenderPipeline : RenderPipeline
{

    private CommandBuffer _cb;
    private FullScreenPlaneGenerator _fullscreen_plane = new FullScreenPlaneGenerator();
    private FullScreenMaterialGenerator _fullscreen_material = new FullScreenMaterialGenerator();

    public CustomRenderPipeline()
        : base()
    {
        //_fullscreen_material.SetTexture(Resources.Load<Texture>("picture"));
    }

    protected CommandBuffer _CB
    {
        get
        {
            if (_cb == null)
            {
                _cb = new CommandBuffer();
            }

            return _cb;
        }
    }

    public override void Dispose()
    {
        base.Dispose();

        if (_cb != null)
        {
            _cb.Dispose();
            _cb = null;
        }
    }

    public override void Render(ScriptableRenderContext renderContext, Camera[] cameras)
    {
        base.Render(renderContext, cameras);

        foreach (var camera in cameras)
        {

            if (camera.CompareTag("MainCamera"))
            {
                RenderMainCamera(renderContext, camera);
            }
            else
            {
                RenderOtherCamera(renderContext, camera);
            }
        }
    }

    private void RenderMainCamera(ScriptableRenderContext renderContext, Camera camera)
    {
        renderContext.SetupCameraProperties(camera);
        _CB.ClearRenderTarget(true, true, Color.blue);
        _CB.DrawMesh(_fullscreen_plane.GetPlane(), Matrix4x4.identity, _fullscreen_material.GetMaterial());
        renderContext.ExecuteCommandBuffer(_CB);
        _CB.Clear();

        renderContext.Submit();
    }

    private void RenderOtherCamera(ScriptableRenderContext renderContext, Camera camera)
    {
        renderContext.SetupCameraProperties(camera);

        _CB.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
        _CB.ClearRenderTarget(true, true, camera.backgroundColor);
        renderContext.ExecuteCommandBuffer(_CB);
        _CB.Clear();
        renderContext.DrawSkybox(camera);

        var cullResults = new CullResults();
        if(CullResults.Cull(camera, renderContext, out cullResults))
        {
             
            var fs = new FilterRenderersSettings(true);
            fs.renderQueueRange = RenderQueueRange.all;
            fs.layerMask = ~0;

            var ds = new DrawRendererSettings(camera, new ShaderPassName("Unlit"));
            ds.sorting.flags = SortFlags.CommonOpaque;

            renderContext.DrawRenderers(cullResults.visibleRenderers, ref ds, fs);
        }

        renderContext.Submit();
    }
}
